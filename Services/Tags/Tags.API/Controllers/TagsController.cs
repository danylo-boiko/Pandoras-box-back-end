using LS.Helpers.Hosting.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tags.Core.CQRS.Commands.CreateTag;
using Tags.Core.CQRS.Commands.DeleteTag;
using Tags.Core.CQRS.Commands.UpdateTag;
using Tags.Core.CQRS.Queries.GetTagById;
using Tags.Core.CQRS.Queries.GetTagsByPattern;
using Tags.Core.Models;

namespace Tags.API.Controllers;

[ApiController]
[Route("api/v1.0/tags")]
public class TagsController : Controller
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetTagByIdQuery
        {
            Id = id
        });
        
        return this.FromExecutionResult(result);
    }
    
    [HttpGet("pattern/{pattern}")]
    public async Task<IActionResult> GetByPattern([FromRoute] string pattern, [FromQuery]PaginationFilter paginationFilter)
    {
        var result = await _mediator.Send(new GetTagsByPatternQuery
        {
            Pattern = pattern,
            PaginationFilter = paginationFilter
        });
        
        return this.FromExecutionResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTagCommand command)
    {
        var result = await _mediator.Send(command);
        
        return this.FromExecutionResult(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] UpdateTagCommand command)
    {
        var result = await _mediator.Send(command);
        
        return this.FromExecutionResult(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await _mediator.Send(new DeleteTagCommand
        {
            Id = id
        });
        
        return this.FromExecutionResult(result);
    }
}