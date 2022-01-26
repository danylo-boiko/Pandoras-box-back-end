namespace Auth.API.Validators
{
    using System.Data;
    using Core.CQRS.Commands.SignUp;
    using FluentValidation;

    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(e => e.Email)
                .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")
                .WithMessage("Wrong email format!");

            RuleFor(e => e.Password)
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")
                .WithMessage("Password must consist of lower and uppercase letters, at least one number, one special character(@$!%*?&) and be 8+ characters long");

            RuleFor(e => e.EmailCode)
                .Must(e => e.Length == 6);

            RuleFor(e => e.BirthDate)
                .Must(e => e.Year - DateTime.UtcNow.Year == 14);
        }
    }
}
