using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Videos.Core.Database;
using Videos.Core.Database.Entities;

namespace Videos.Core.CQRS.Queries.GetVideoById;

public class GetVideoByIdQueryHandler : IRequestHandler<GetVideoByIdQuery, ExecutionResult<Video>>
{
    private readonly ILogger<GetVideoByIdQueryHandler> _logger;
    private readonly VideosDbContext _videosDbContext;

    public GetVideoByIdQueryHandler(ILogger<GetVideoByIdQueryHandler> logger, VideosDbContext videosDbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                _logger.LogError("Video with id: {Id} is not exist", request.Id);
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