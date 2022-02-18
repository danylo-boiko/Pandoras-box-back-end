using FluentValidation;

namespace Tags.Core.CQRS.Commands.UpdateTag;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(t => t.Content)
            .NotEmpty().WithMessage("{Content} is required.")
            .MinimumLength(3).WithMessage("{Content} must be at least 3 characters")
            .MaximumLength(32).WithMessage("{Content} must not exceed 32 characters");
    }
}