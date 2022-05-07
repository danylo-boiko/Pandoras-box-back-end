using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Videos.Core.Database;
using Videos.Core.Database.Entities;
using Videos.Core.GrpcServices;

namespace Videos.Core.CQRS.Commands.UpdateVideo;

public class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, ExecutionResult<Video>>
{
    private readonly TagsGrpcService _tagsGrpcService;
    private readonly VideosDbContext _videosDbContext;

    public UpdateVideoCommandHandler(
        TagsGrpcService tagsGrpcService,
        VideosDbContext videosDbContext
    )
    {
        _tagsGrpcService = tagsGrpcService ?? throw new ArgumentNullException(nameof(tagsGrpcService));
        _videosDbContext = videosDbContext ?? throw new ArgumentNullException(nameof(videosDbContext));
    }

    public async Task<ExecutionResult<Video>> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existVideo = await _videosDbContext.Videos.Include(v => v.VideoTags)
                .FirstOrDefaultAsync(v => v.Id.Equals(request.Id));

            if (existVideo is null)
            {
                return new ExecutionResult<Video>(new ErrorInfo($"Video with id: {request.Id} is not exist."));
            }

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

            if (request.Description is not null)
            {
                existVideo.Description = request.Description;
            }

            existVideo.VideoTags.Clear();
            newVideoTags.ForEach(vt => existVideo.VideoTags.Add(vt));
            await _videosDbContext.SaveChangesAsync();

            return new ExecutionResult<Video>(new InfoMessage("Video has been updated successfully."));
        }
        catch (Exception e)
        {
            return new ExecutionResult<Video>(new ErrorInfo($"Error while updating a new video. {e.Message}"));
        }
    }
}