using Microsoft.AspNetCore.Http;

namespace Videos.Core.CQRS.Commands.CreateVideo;

public record CreateVideoCommand(int AuthorId, IFormFile Video, string? Description);