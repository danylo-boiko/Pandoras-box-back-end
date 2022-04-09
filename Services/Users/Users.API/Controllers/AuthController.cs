using Users.Core.CQRS.Commands.Auth.SendTwoFactorDigitCode;
using Users.Core.CQRS.Commands.Auth.SignIn;
using Users.Core.CQRS.Commands.Auth.SignOut;
using Users.Core.CQRS.Commands.Auth.SignUp;

namespace Users.API.Controllers
{
    using LS.Helpers.Hosting.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1.0/auth")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Send e-mail confirmation code to the given address.
        /// </summary>
        /// <param name="command">E-mail address data.</param>
        /// <returns></returns>
        [HttpPost("e-mail-confirmation-code")]
        public async Task<IActionResult> SendEmailConfirmationCode([FromBody] SendTwoFactorDigitCodeCommand command)
        {
            var result = await _mediator.Send(command);

            return this.FromExecutionResult(result);
        }

        /// <summary>
        /// Sign up a new user.
        /// </summary>
        /// <param name="command">New user data.</param>
        /// <returns></returns>
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var result = await _mediator.Send(command);

            return this.FromExecutionResult(result);
        }

        /// <summary>
        /// Log in as an existing user.
        /// </summary>
        /// <param name="command">Log in credentials.</param>
        /// <returns></returns>
        [HttpPost("log-in")]
        public async Task<IActionResult> LogIn([FromBody] SignInCommand command)
        {
            var result = await _mediator.Send(command);

            return this.FromExecutionResult(result);
        }

        /// <summary>
        /// Log out from an account.
        /// </summary>
        /// <returns></returns>
        [HttpPost("log-out")]
        public async Task<IActionResult> LogOut()
        {
            var command = new SignOutCommand();
            var result = await _mediator.Send(command);

            return this.FromExecutionResult(result);
        }
    }
}