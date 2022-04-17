using Users.Core.CQRS.Commands.Profile.SetProfileInfo;
using Users.Core.CQRS.Queries.GetProfileAvatar;
using Users.Core.CQRS.Queries.GetProfileInfo;

namespace Users.API.Controllers
{
    using Core.CQRS.Commands.Profile.SetAvatar;
    using Core.Models.Request.Profile;
    using LS.Helpers.Hosting.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1.0/profile")]
    [Authorize(Roles = "User, Admin")]
    public class ProfileController : Controller
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Set profile avatar.
        /// </summary>
        /// <param name="request">Avatar data.</param>
        /// <returns></returns>
        [HttpPost("avatar")]
        public async Task<IActionResult> SetAvatarAsync([FromForm] SetAvatarRequest request)
        {
            var command = new SetAvatarCommand { Avatar = request.Avatar };
            var result = await _mediator.Send(command);
            return this.FromExecutionResult(result);
        }

        /// <summary>
        /// Set profile info.
        /// </summary>
        /// <param name="request">Avatar data.</param>
        /// <returns></returns>
        [HttpPut("info")]
        public async Task<IActionResult> SetProfileInfoAsync([FromBody] SetProfileInfoRequest request)
        {
            var command = new SetProfileInfoCommand
            {
                Bio = request.Bio,
                DisplayName = request.DisplayName
            };

            var result = await _mediator.Send(command);
            return this.FromExecutionResult(result);
        }

        /// <summary>
        /// Get profile info.
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        public async Task<IActionResult> GetProfileInfoAsync()
        {
            var query = new GetProfileInfoQuery();
            var result = await _mediator.Send(query);
            return this.FromExecutionResult(result);
        }

        /// <summary>
        /// Get profile avatar.
        /// </summary>
        /// <returns></returns>
        [HttpGet("avatar")]
        public async Task<IActionResult> GetAvatarAsync()
        {
            var query = new GetProfileAvatarQuery();
            var result = await _mediator.Send(query);
            return !result.Success ?
                this.FromExecutionResult(result) :
                new FileStreamResult(result.Value.AvatarStream, $"image/{result.Value.Extension}");
        }
    }
}
