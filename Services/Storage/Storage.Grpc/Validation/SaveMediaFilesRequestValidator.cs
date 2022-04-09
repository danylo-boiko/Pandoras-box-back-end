using Storage.Core.Enums;

namespace Storage.Grpc.Validation
{
    using FluentValidation;

    public class SaveMediaFilesRequestValidator : AbstractValidator<SaveMediaFilesRequest>
    {
        private readonly string[] _validFileExtensions = { ".jpg", ".jpeg", ".png", ".mp4", ".wmv", ".mov", ".avi", ".webm" };

        public SaveMediaFilesRequestValidator()
        {
            RuleFor(e => e.Extension)
                .Must(e => _validFileExtensions.Contains(e))
                .WithMessage("Unsupported file format.");

            RuleFor(e => e.CategoryId)
                .Must(e => Enum.IsDefined(typeof(FileCategory), e))
                .WithMessage("Invalid file category.");

            RuleFor(e => e.FileBytes)
                .NotEmpty()
                .Must(e => !e.IsEmpty)
                .WithMessage("Actual file data was not sent.");
        }
    }
}
