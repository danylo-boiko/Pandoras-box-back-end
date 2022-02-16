﻿using AutoMapper;
using LS.Helpers.Hosting.API;
using MediatR;
using Tags.Core.GrpcServices;
using Tags.Core.Models;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.CQRS.Commands.CreateTag;

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, ExecutionResult<Tag>>
{
    private readonly ITagsRepository _tagsRepository;
    private readonly IMapper _mapper;
    private readonly UsersGrpcService _usersGrpcService;
    
    public CreateTagCommandHandler(ITagsRepository tagsRepository, IMapper mapper, UsersGrpcService usersGrpcService)
    {
        _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _usersGrpcService = usersGrpcService ?? throw new ArgumentNullException(nameof(usersGrpcService));
    }
    
    public async Task<ExecutionResult<Tag>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _tagsRepository.GetAsync(request.Content) is not null)
            {
                return new ExecutionResult<Tag>(new ErrorInfo($"Tag with content: {request.Content} already exist."));
            }

            if (await _usersGrpcService.GetUserAsync(request.AuthorId) is null)
            {
                return new ExecutionResult<Tag>(new ErrorInfo($"Author with id: {request.AuthorId} is not exist."));
            }
            
            var tagEntity = _mapper.Map<Tag>(request);
            
            await _tagsRepository.CreateAsync(tagEntity);
            
            return new ExecutionResult<Tag>(tagEntity);
        }
        catch (Exception e)
        {
            return new ExecutionResult<Tag>(new ErrorInfo("Error while trying to create a tag.", e.Message));
        }
    }
}