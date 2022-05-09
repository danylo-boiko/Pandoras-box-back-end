using Grpc.Core;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Videos.Core.Database;
using Videos.Core.Database.Entities;
using Videos.Core.GrpcServices;

namespace Videos.Core.CQRS.Commands.UpdateVideo;

public class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, ExecutionResult<Video>>
{
    private readonly ILogger<UpdateVideoCommandHandler> _logger;
    private readonly TagsGrpcService _tagsGrpcService;
    private readonly VideosDbContext _videosDbContext;

    public UpdateVideoCommandHandler(
        ILogger<UpdateVideoCommandHandler> logger, 
        TagsGrpcService tagsGrpcService, 
        VideosDbContext videosDbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tagsGrpcService = tagsGrpcService ?? throw new ArgumentNullException(nameof(tagsGrpcService));
        _videosDbContext = videosDbContext ?? throw new ArgumentNullException(nameof(videosDbContext));
    }

    public async Task<ExecutionResult<Video>> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existVideo = await _videosDbContext.Videos
                .Include(v => v.VideoTags)
                .FirstOrDefaultAsync(v => v.Id.Equals(request.Id));

            if (existVideo is null)
            {
                _logger.LogError("Video with id: {Id} is not exist", request.Id);
                return new ExecutionResult<Video>(new ErrorInfo($"Video with id: {request.Id} is not exist."));
            }

            if (request.Description is not null)
            {
                existVideo.Description = request.Description;
            }

            if (request.TagsIds is not null)
            {
                var newVideoTags = new List<VideoTag>();
            
                foreach (var tagId in request.TagsIds)
                {
                    await _tagsGrpcService.GetTagAsync(tagId);
                    newVideoTags.Add(new VideoTag
                    {
                        TagId = tagId,
                        VideoId = request.Id
                    });
                }

                existVideo.VideoTags.Clear();
                newVideoTags.ForEach(vt => existVideo.VideoTags.Add(vt));
            }
            
            await _videosDbContext.SaveChangesAsync();
            
            _logger.LogInformation("Video with id: {Id} has been successfully updated", request.Id);
            return new ExecutionResult<Video>(new InfoMessage($"Video with id: {request.Id} has been updated successfully."));
        }
        catch (RpcException e)
        {
            return new ExecutionResult<Video>(new ErrorInfo("gRPC server error.", e.Status.Detail));
        }
        catch (Exception e)
        {
            return new ExecutionResult<Video>(new ErrorInfo($"Error while updating a new video. {e.Message}"));
        }
    }
}