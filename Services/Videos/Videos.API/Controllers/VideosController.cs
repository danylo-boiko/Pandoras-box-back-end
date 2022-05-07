using MediatR;
using Microsoft.AspNetCore.Mvc;

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
}