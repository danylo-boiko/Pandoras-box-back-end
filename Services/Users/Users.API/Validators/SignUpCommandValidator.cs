namespace Users.API.Validators
{
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
                .Must(e => DateTime.UtcNow.Year - e.Year > 13)
                .WithMessage("You must be at least 14 years old.");

            RuleFor(e => e.DisplayName)
                .Matches("^(?=.{4,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$")
                .WithMessage("Display name can contain letters, numbers, underscores or dots and be from 4 to 20 characters long.");
        }
    }
}
