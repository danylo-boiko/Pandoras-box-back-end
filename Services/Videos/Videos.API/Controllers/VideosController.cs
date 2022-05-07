using LS.Helpers.Hosting.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Videos.Core.CQRS.Commands.CreateVideo;

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
    
    [HttpPost]
    public async Task<IActionResult> Create([FromForm]CreateVideoCommand command)
    {
        var result = await _mediator.Send(command);
        
        return this.FromExecutionResult(result);
    }
}