using AutoMapper;
using Grpc.Core;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Logging;
using Tags.Core.Database.Entities;
using Tags.Core.GrpcServices;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.CQRS.Commands.CreateTag;

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, ExecutionResult<Tag>>
{
    private readonly ILogger<CreateTagCommandHandler> _logger;
    private readonly ITagsRepository _tagsRepository;
    private readonly IMapper _mapper;
    private readonly UsersGrpcService _usersGrpcService;
    
    public CreateTagCommandHandler(
        ILogger<CreateTagCommandHandler> logger, 
        ITagsRepository tagsRepository, 
        IMapper mapper, 
        UsersGrpcService usersGrpcService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _usersGrpcService = usersGrpcService ?? throw new ArgumentNullException(nameof(usersGrpcService));
    }
    
    public async Task<ExecutionResult<Tag>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existTag = await _tagsRepository.GetAsync(request.Content);
            
            if (existTag is not null)
            {
                _logger.LogError("Tag with content {Content} already exist, id: {Id}", request.Content, existTag.Id);
                return new ExecutionResult<Tag>(new ErrorInfo($"Tag with content '{request.Content}' already exist, id: {existTag.Id}"));
            }

            await _usersGrpcService.GetUserAsync(request.AuthorId);
            
            var tagEntity = _mapper.Map<Tag>(request);
            
            await _tagsRepository.CreateAsync(tagEntity);

            _logger.LogInformation("Tag {Tag} (id:{Id}) has been successfully created", tagEntity.Content, tagEntity.Id);
            return new ExecutionResult<Tag>(tagEntity);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                _logger.LogError("gRPC server error: {Message}", e.Status.Detail);
            }
            
            return new ExecutionResult<Tag>(new ErrorInfo("gRPC server error.",e.Status.Detail));
        }
        catch (Exception e)
        {
            return new ExecutionResult<Tag>(new ErrorInfo("Error while trying to create a tag.", e.Message));
        }
    }
}