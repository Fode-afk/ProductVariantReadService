using FluentValidation;
using migApp.Shared.Domain.Errors;

namespace ProductVariantReadService.Application.Features.Queries.GetOwnVariantsByProductId;

public sealed class GetOwnVariantsByProductIdQueryValidator : AbstractValidator<GetOwnVariantsByProductIdQuery>
{
    public GetOwnVariantsByProductIdQueryValidator()
    {
        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3).WithErrorCode(CurrencyErrorCodes.InvalidFormat);
    }
}
