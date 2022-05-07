using LS.Helpers.Hosting.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Videos.Core.CQRS.Commands.CreateVideo;
using Videos.Core.CQRS.Commands.DeleteVideo;
using Videos.Core.CQRS.Queries.GetVideoById;
using Videos.Core.CQRS.Queries.GetVideosByTagId;
using Videos.Core.CQRS.Queries.GetVideosByUserId;
using Videos.Core.Database;

namespace Videos.API.Controllers;

[ApiController]
[Route("api/v1.0/videos")]
public class VideosController : Controller
{
    private readonly IMediator _mediator;

    public VideosController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetVideoByIdQuery
        {
            Id = id
        });
        
        return this.FromExecutionResult(result);
    }
    
    [HttpGet("tag/{tagId}")]
    public async Task<IActionResult> GetByTagId([FromRoute] int tagId, [FromQuery]PaginationFilter paginationFilter)
    {
        var result = await _mediator.Send(new GetVideosByTagIdQuery
        {
            TagId = tagId,
            PaginationFilter = paginationFilter
        });
        
        return this.FromExecutionResult(result);
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId([FromRoute] int userId, [FromQuery]PaginationFilter paginationFilter)
    {
        var result = await _mediator.Send(new GetVideosByUserIdQuery
        {
            UserId = userId,
            PaginationFilter = paginationFilter
        });
        
        return this.FromExecutionResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromForm]CreateVideoCommand command)
    {
        var result = await _mediator.Send(command);
        
        return this.FromExecutionResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await _mediator.Send(new DeleteVideoCommand
        {
            Id = id
        });

        return this.FromExecutionResult(result);
    }
}