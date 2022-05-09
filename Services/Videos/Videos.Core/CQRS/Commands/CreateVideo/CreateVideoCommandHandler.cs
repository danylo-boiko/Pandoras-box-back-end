﻿using Calzolari.Grpc.Net.Client.Validation;
using Grpc.Core;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Logging;
using Videos.Core.Database;
using Videos.Core.Database.Entities;
using Videos.Core.GrpcServices;

namespace Videos.Core.CQRS.Commands.CreateVideo;

public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, ExecutionResult<Video>>
{
    private readonly ILogger<CreateVideoCommandHandler> _logger;
    private readonly StorageGrpcService _storageGrpcService;
    private readonly UsersGrpcService _usersGrpcService;
    private readonly TagsGrpcService _tagsGrpcService;
    private readonly VideosDbContext _videosDbContext;

    public CreateVideoCommandHandler(
        ILogger<CreateVideoCommandHandler> logger,
        StorageGrpcService storageGrpcService,
        UsersGrpcService usersGrpcService,
        TagsGrpcService tagsGrpcService,
        VideosDbContext videosDbContext
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _storageGrpcService = storageGrpcService ?? throw new ArgumentNullException(nameof(storageGrpcService));
        _usersGrpcService = usersGrpcService ?? throw new ArgumentNullException(nameof(usersGrpcService));
        _tagsGrpcService = tagsGrpcService ?? throw new ArgumentNullException(nameof(tagsGrpcService));
        _videosDbContext = videosDbContext ?? throw new ArgumentNullException(nameof(videosDbContext));
    }

    public async Task<ExecutionResult<Video>> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            int videoId = 0;
            await _usersGrpcService.GetUserAsync(request.AuthorId);

            foreach (var tagId in request.TagsIds)
            {
                await _tagsGrpcService.GetTagAsync(tagId);
            }

            //todo NSFW detection for new video

            var saveVideoResponse = await _storageGrpcService.SaveVideo(request.AuthorId, request.Video);

            if (saveVideoResponse.IsSuccess)
            {
                await using var transaction = await _videosDbContext.Database.BeginTransactionAsync();
                
                try
                {
                    var video = new Video
                    {
                        AuthorId = request.AuthorId,
                        Description = request.Description,
                        CreatedAt = DateTime.Now,
                        VideoUrl = saveVideoResponse.Locations.First()
                    };

                    _videosDbContext.Videos.Add(video);
                    await _videosDbContext.SaveChangesAsync();
                    videoId = video.Id;
                    
                    foreach (var tagId in request.TagsIds)
                    {
                        var videoTag = new VideoTag
                        {
                            VideoId = video.Id,
                            TagId = tagId
                        };

                        _videosDbContext.VideoTags.Add(videoTag);
                        await _videosDbContext.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                }
            }

            if (saveVideoResponse.IsSuccess)
            {
                _logger.LogInformation("Video has been uploaded successfully, id {Id}", videoId);
                return new ExecutionResult<Video>(new InfoMessage($"Video has been uploaded successfully, id {videoId}."));
            }

            return new ExecutionResult<Video>(new ErrorInfo(saveVideoResponse.Message));
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.InvalidArgument)
            {
                var errorsInfo = new List<ErrorInfo>();
                foreach (var error in e.GetValidationErrors())
                {
                    errorsInfo.Add(new ErrorInfo(error.PropertyName, error.ErrorMessage));
                }

                return new ExecutionResult<Video>(errorsInfo);
            }

            return new ExecutionResult<Video>(new ErrorInfo("gRPC server error.", e.Status.Detail));
        }
        catch (Exception e)
        {
            return new ExecutionResult<Video>(new ErrorInfo($"Error while uploading a new video. {e.Message}"));
        }
    }
}