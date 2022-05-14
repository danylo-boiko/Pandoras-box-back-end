using FluentValidation;

namespace Tags.Core.CQRS.Commands.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(t => t.AuthorId)
            .NotEmpty();
        
        RuleFor(t => t.Content)
            .NotEmpty().WithMessage("{Content} is required.")
            .MinimumLength(3).WithMessage("{Content} must be at least 3 characters")
            .MaximumLength(32).WithMessage("{Content} must not exceed 32 characters");
    }
}