using FluentValidation;

namespace Videos.Core.CQRS.Commands.CreateVideo;

public class CreateVideoCommandValidator : AbstractValidator<CreateVideoCommand>
{
    public CreateVideoCommandValidator()
    {
        RuleFor(v => v.AuthorId)
            .NotEmpty();

        RuleFor(v => v.Video)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(250);
        
        RuleFor(v => v.TagsIds)
            .NotEmpty()
            .Must(v => v.Count == v.ToHashSet().Count)
            .When(v => v.TagsIds != null)
            .WithMessage("Tags ids is not unique.");
    }
}