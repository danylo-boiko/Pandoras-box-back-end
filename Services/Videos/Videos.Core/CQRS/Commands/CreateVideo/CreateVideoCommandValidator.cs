using FluentValidation;

namespace Videos.Core.CQRS.Commands.CreateVideo;

public class CreateVideoCommandValidator : AbstractValidator<CreateVideoCommand>
{
    private readonly string[] _validVideoExtensions = { ".mp4", ".wmv", ".mov", ".avi", ".webm" };

    public CreateVideoCommandValidator()
    {
        RuleFor(v => v.Video)
            .Must(v => _validVideoExtensions.Contains(Path.GetExtension(v.FileName)))
            .WithMessage("Unsupported video format.");
        
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