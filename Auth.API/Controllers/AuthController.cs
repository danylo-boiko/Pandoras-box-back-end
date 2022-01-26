namespace Auth.API.Controllers
{
    using Core.CQRS.Commands.SendTwoFactorDigitCode;
    using Core.CQRS.Commands.SignUp;
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

        [HttpPost("email-confirmation-code")]
        public async Task<IActionResult> SignUp([FromBody] SendTwoFactorDigitCodeCommand command)
        {
            var result = await _mediator.Send(command);

            return this.FromExecutionResult(result);
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var result = await  _mediator.Send(command);

            return this.FromExecutionResult(result);
        }
    }
}