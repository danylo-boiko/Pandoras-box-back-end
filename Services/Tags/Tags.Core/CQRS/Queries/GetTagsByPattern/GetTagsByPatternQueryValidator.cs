using FluentValidation;

namespace Tags.Core.CQRS.Queries.GetTagsByPattern;

public class GetTagsByPatternQueryValidator : AbstractValidator<GetTagsByPatternQuery>
{
    public GetTagsByPatternQueryValidator()
    {
        RuleFor(t => t.Pattern)
            .NotEmpty().WithMessage("{Content} is required.");
    }
}