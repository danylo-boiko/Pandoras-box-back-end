using FluentValidation;

namespace Tags.Core.CQRS.Queries.GetTagsByPattern;

public class GetTagsByPatternValidator : AbstractValidator<GetTagsByPatternQuery>
{
    public GetTagsByPatternValidator()
    {
        RuleFor(t => t.Pattern)
            .NotEmpty().WithMessage("{Content} is required.");
    }
}