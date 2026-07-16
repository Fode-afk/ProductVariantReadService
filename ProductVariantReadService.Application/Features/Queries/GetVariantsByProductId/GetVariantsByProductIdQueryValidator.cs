using FluentValidation;
using migApp.Shared.Domain.Errors;

namespace ProductVariantReadService.Application.Features.Queries.GetVariantsByProductId;

public sealed class GetVariantsByProductIdQueryValidator : AbstractValidator<GetVariantsByProductIdQuery>
{
    public GetVariantsByProductIdQueryValidator()
    {
        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3).WithErrorCode(CurrencyErrorCodes.InvalidFormat);
    }
}