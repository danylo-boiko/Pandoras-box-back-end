using FluentValidation;

namespace Videos.Core.CQRS.Commands.UpdateVideo;

public class UpdateVideoCommandValidator : AbstractValidator<UpdateVideoCommand>
{
    public UpdateVideoCommandValidator()
    {
        RuleFor(v => v.Description)
            .MaximumLength(250);
        
        RuleFor(v => v.TagsIds)
            .Must(v => v.Count == v.ToHashSet().Count)
            .When(v => v.TagsIds != null)
            .WithMessage("Tags ids is not unique.");
    }
}