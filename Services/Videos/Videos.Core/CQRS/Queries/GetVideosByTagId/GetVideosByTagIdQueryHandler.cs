using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Videos.Core.Database;
using Videos.Core.Database.Entities;

namespace Videos.Core.CQRS.Queries.GetVideosByTagId;

public class GetVideosByTagIdQueryHandler : IRequestHandler<GetVideosByTagIdQuery, ExecutionResult<IList<Video>>>
{
    private readonly VideosDbContext _videosDbContext;

    public GetVideosByTagIdQueryHandler(VideosDbContext videosDbContext)
    {
        _videosDbContext = videosDbContext ?? throw new ArgumentNullException(nameof(videosDbContext));
    }
    
    public async Task<ExecutionResult<IList<Video>>> Handle(GetVideosByTagIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var videos = _videosDbContext.Videos.Where(v => v.VideoTags.Any(vt => vt.TagId.Equals(request.TagId)));

            if (request.PaginationFilter.Limit == 0)
            {
                videos = videos.Skip(request.PaginationFilter.Offset);
            }
            else
            {
                videos = videos.Skip(request.PaginationFilter.Offset).Take(request.PaginationFilter.Limit);
            }
            
            return new ExecutionResult<IList<Video>>(await videos.ToListAsync());
        }
        catch (Exception e)
        {
            return new ExecutionResult<IList<Video>>(new ErrorInfo("Error while trying to get a video.", e.Message));
        }
    }
}