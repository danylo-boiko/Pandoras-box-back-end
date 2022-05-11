using Calzolari.Grpc.Net.Client.Validation;
using EventBus.Messages.Events;
using Grpc.Core;
using LS.Helpers.Hosting.API;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Videos.Core.Database;
using Videos.Core.Database.Entities;
using Videos.Core.Enums;
using Videos.Core.GrpcServices;

namespace Videos.Core.CQRS.Commands.CreateVideo;

public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, ExecutionResult<Video>>
{
    private readonly ILogger<CreateVideoCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly StorageGrpcService _storageGrpcService;
    private readonly UsersGrpcService _usersGrpcService;
    private readonly TagsGrpcService _tagsGrpcService;
    private readonly VideosDbContext _videosDbContext;

    public CreateVideoCommandHandler(
        ILogger<CreateVideoCommandHandler> logger,
        IPublishEndpoint publishEndpoint,
        StorageGrpcService storageGrpcService,
        UsersGrpcService usersGrpcService,
        TagsGrpcService tagsGrpcService,
        VideosDbContext videosDbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _storageGrpcService = storageGrpcService ?? throw new ArgumentNullException(nameof(storageGrpcService));
        _usersGrpcService = usersGrpcService ?? throw new ArgumentNullException(nameof(usersGrpcService));
        _tagsGrpcService = tagsGrpcService ?? throw new ArgumentNullException(nameof(tagsGrpcService));
        _videosDbContext = videosDbContext ?? throw new ArgumentNullException(nameof(videosDbContext));
    }

    public async Task<ExecutionResult<Video>> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _usersGrpcService.GetUserAsync(request.AuthorId);

            foreach (var tagId in request.TagsIds)
            {
                await _tagsGrpcService.GetTagAsync(tagId);
            }

            var saveVideoResponse = await _storageGrpcService.SaveVideo(request.AuthorId, request.Video);

            var video = new Video
            {
                AuthorId = request.AuthorId,
                VideoUrl = saveVideoResponse.Locations.First(),
                ClassificationStatus = ClassificationStatus.UnReviewed,
                Description = request.Description,
                CreatedAt = DateTime.Now
            };
            
            if (saveVideoResponse.IsSuccess)
            {
                await using var transaction = await _videosDbContext.Database.BeginTransactionAsync();
                try
                {
                    _videosDbContext.Videos.Add(video);
                    await _videosDbContext.SaveChangesAsync();
                    
                    foreach (var tagId in request.TagsIds)
                    {
                        _videosDbContext.VideoTags.Add(new VideoTag
                        {
                            VideoId = video.Id,
                            TagId = tagId
                        });
                    }

                    await _videosDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                }
            }

            if (saveVideoResponse.IsSuccess)
            {
                var detectionEvent = new NsfwVideoDetectionEvent
                {
                    VideoId = video.Id,
                    AuthorId = video.AuthorId,
                    VideoLocation = video.VideoUrl
                };
                
                await _publishEndpoint.Publish<NsfwVideoDetectionEvent>(detectionEvent);
                
                _logger.LogInformation("Video has been uploaded successfully, id {Id}", video.Id);
                return new ExecutionResult<Video>(new InfoMessage($"Video has been uploaded successfully, id {video.Id}."));
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
                    _logger.LogError("Grpc validation error: {Error}", error.ErrorMessage);
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