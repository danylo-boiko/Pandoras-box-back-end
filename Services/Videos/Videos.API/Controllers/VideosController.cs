using Microsoft.AspNetCore.Mvc;
using Videos.API.GrpcServices;
using Videos.Core.CQRS.Commands.CreateVideo;

namespace Videos.API.Controllers;

[ApiController]
[Route("api/v1.0/videos")]
public class VideosController : Controller
{
    private readonly NsfwDetectionGrpcService _nsfwDetectionGrpcService;

    public VideosController(NsfwDetectionGrpcService nsfwDetectionGrpcService)
    {
        _nsfwDetectionGrpcService = nsfwDetectionGrpcService;
    }
    
    [HttpPost]
    [RequestSizeLimit(104857600)]
    public async Task<IActionResult> Create([FromForm]CreateVideoCommand command)
    {
        return Ok(await _nsfwDetectionGrpcService.DetectFromVideo(command.Video));
    }
}