namespace Users.API.Controllers
{
    using Core.CQRS.Commands.Profile.SetAvatar;
    using Core.Models.Request.Profile;
    using LS.Helpers.Hosting.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1.0/users/{id:int}/profile")]
    [Authorize(Roles = "User, Admin")]
    public class ProfileController : Controller
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> SetAvatarAsync(
            int id,
            [FromForm] SetAvatarRequest request)
        {
            var command = new SetAvatarCommand { Avatar = request.Avatar };
            var result = await _mediator.Send(command);
            return this.FromExecutionResult(result);
        }
    }
}
