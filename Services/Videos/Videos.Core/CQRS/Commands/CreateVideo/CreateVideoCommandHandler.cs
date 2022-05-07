using LS.Helpers.Hosting.API;
using MediatR;
using Videos.Core.Database;
using Videos.Core.Database.Entities;
using Videos.Core.GrpcServices;

namespace Videos.Core.CQRS.Commands.CreateVideo;

public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, ExecutionResult<Video>>
{
    private readonly StorageGrpcService _storageGrpcService;
    private readonly UsersGrpcService _usersGrpcService;
    private readonly TagsGrpcService _tagsGrpcService;
    private readonly VideosDbContext _videosDbContext;

    public CreateVideoCommandHandler(
        StorageGrpcService storageGrpcService,
        UsersGrpcService usersGrpcService,
        TagsGrpcService tagsGrpcService,
        VideosDbContext videosDbContext
    )
    {
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
            
            return saveVideoResponse.IsSuccess
                ? new ExecutionResult<Video>(new InfoMessage("Video has been uploaded successfully."))
                : new ExecutionResult<Video>(new ErrorInfo(saveVideoResponse.Message));
        }
        catch (Exception e)
        {
            return new ExecutionResult<Video>(new ErrorInfo($"Error while uploading a new video. {e.Message}"));
        }
    }
}