using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Videos.Core.Database;
using Videos.Core.Database.Entities;

namespace Videos.Core.CQRS.Queries.GetVideoById;

public class GetVideoByIdQueryHandler : IRequestHandler<GetVideoByIdQuery, ExecutionResult<Video>>
{
    private readonly VideosDbContext _videosDbContext;

    public GetVideoByIdQueryHandler(VideosDbContext videosDbContext)
    {
        _videosDbContext = videosDbContext ?? throw new ArgumentNullException(nameof(videosDbContext));
    }

    public async Task<ExecutionResult<Video>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var video = await _videosDbContext.Videos
                .Include(v => v.VideoTags)
                .FirstOrDefaultAsync(v => v.Id.Equals(request.Id));

            if (video is null)
            {
                return new ExecutionResult<Video>(new ErrorInfo($"Video with id: {request.Id} is not exist."));
            }

            return new ExecutionResult<Video>(video);
        }
        catch (Exception e)
        {
            return new ExecutionResult<Video>(new ErrorInfo("Error while trying to get a video.", e.Message));
        }
    }
}