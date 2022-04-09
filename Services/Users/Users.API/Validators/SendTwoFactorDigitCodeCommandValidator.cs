using Users.Core.CQRS.Commands.Auth.SendTwoFactorDigitCode;

namespace Users.API.Validators
{
    using FluentValidation;

    public class SendTwoFactorDigitCodeCommandValidator : AbstractValidator<SendTwoFactorDigitCodeCommand>
    {
        public SendTwoFactorDigitCodeCommandValidator()
        {
            RuleFor(e => e.Email)
                .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")
                .WithMessage("Wrong email format!");
        }
    }
}
