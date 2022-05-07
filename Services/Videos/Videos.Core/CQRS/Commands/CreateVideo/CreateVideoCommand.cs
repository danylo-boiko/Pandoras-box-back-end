using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Http;
using Videos.Core.Database.Entities;

namespace Videos.Core.CQRS.Commands.CreateVideo;

public record CreateVideoCommand : IRequest<ExecutionResult<Video>>
{
    public int AuthorId { get; set; }
    public IFormFile Video { get; set; }
    public string? Description { get; set; }
    public ICollection<int> TagsIds { get; set; }
}