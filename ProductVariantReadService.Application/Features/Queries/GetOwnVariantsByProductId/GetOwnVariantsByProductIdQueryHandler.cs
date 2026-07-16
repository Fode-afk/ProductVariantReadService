using MediatR;
using migApp.Shared.Domain.ValueObjects;
using migApp.Shared.Results;
using ProductVariantReadService.Application.Caching;
using ProductVariantReadService.Application.Dtos;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Application.Interfaces.Metrics;
using ProductVariantReadService.Application.Interfaces.Services;
using ProductVariantReadService.Application.Mapping;
using ProductVariantReadService.Domain.Errors;
using ProductVariantReadService.Domain.Models;
using ZiggyCreatures.Caching.Fusion;
using static migApp.Shared.Results.ResultFactory;

namespace ProductVariantReadService.Application.Features.Queries.GetOwnVariantsByProductId;

public sealed class GetOwnVariantsByProductIdQueryHandler(
    IProductVariantReadRepository productReadRepository,
    IFusionCache cache,
    IProductVariantReadMetrics metrics,
    IMoneyConverter moneyConverter) : IRequestHandler<GetOwnVariantsByProductIdQuery, IResult<IEnumerable<ProductVariantOwnerDto>>>
{
    public async Task<IResult<IEnumerable<ProductVariantOwnerDto>>> Handle(GetOwnVariantsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var currencyResult = Currency.Create(request.Currency);
        if (currencyResult.IsFailure)
            return Fail<IEnumerable<ProductVariantOwnerDto>>(currencyResult.Error);

        var currency = currencyResult.Value;
        var wasHit = true;

        var variantDtos = await cache.GetOrSetAsync<IEnumerable<ProductVariantOwnerDto>?>(
            CacheKeys.OwnVariantsByProductId(request.ProductId, currency.Code),
            async (entry, ct) =>
            {
                wasHit = false;

                var variants = await productReadRepository.GetByProductIdAsync(request.ProductId, ct);

                if (variants is null || !variants.Any())
                    return null;

                entry.Tags = [CacheTags.VariantsByProductId(request.ProductId)];

                var dtos = new List<ProductVariantOwnerDto>();

                foreach (var variant in variants)
                {
                    var dtoResult = await ToConvertedOwnerDtoAsync(variant, currency, ct);

                    if (dtoResult.IsFailure)
                        return null;

                    dtos.Add(dtoResult.Value);
                }

                return dtos;
            },
            token: cancellationToken);

        metrics.RecordCacheHitOrMiss("own-product-variant-by-product-id", wasHit);

        if (variantDtos is null || !variantDtos.Any())
            return Fail<IEnumerable<ProductVariantOwnerDto>>(ProductVariantErrors.NotFound());

        if (!variantDtos.All(v => v.VendorId == request.VendorId))
            return Fail<IEnumerable<ProductVariantOwnerDto>>(ProductVariantErrors.NotFound());

        return Ok(variantDtos);
    }

    private async Task<IResult<ProductVariantOwnerDto>> ToConvertedOwnerDtoAsync(
        ProductVariantDocument document,
        Currency targetCurrency,
        CancellationToken cancellationToken)
    {
        PriceInfoDto? priceDto = null;

        if (document.Price is not null)
        {
            var amountResult = await ConvertUsdToTargetMinorAsync(
                document.Price.AmountMinor, targetCurrency, cancellationToken);

            if (amountResult.IsFailure)
                return Fail<ProductVariantOwnerDto>(amountResult.Error);

            long? oldAmountMinor = null;

            if (document.Price.OldAmountMinor is not null)
            {
                var oldAmountResult = await ConvertUsdToTargetMinorAsync(
                    document.Price.OldAmountMinor.Value, targetCurrency, cancellationToken);

                if (oldAmountResult.IsFailure)
                    return Fail<ProductVariantOwnerDto>(oldAmountResult.Error);

                oldAmountMinor = oldAmountResult.Value;
            }

            priceDto = new PriceInfoDto(amountResult.Value, oldAmountMinor);
        }

        return Ok(document.ToOwnerDto() with { Price = priceDto });
    }

    private async Task<IResult<long>> ConvertUsdToTargetMinorAsync(
        long usdAmountMinor,
        Currency targetCurrency,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var usdMoneyResult = Money.FromMinor(usdAmountMinor, Currency.USD);
        if (usdMoneyResult.IsFailure)
            return Fail<long>(usdMoneyResult.Error);

        var money = usdMoneyResult.Value;

        if (targetCurrency != Currency.USD)
        {
            var converted = await moneyConverter.ConvertAsync(
                money,
                targetCurrency,
                cancellationToken);

            if (converted.IsFailure)
                return Fail<long>(converted.Error);

            money = converted.Value;
        }

        var minorResult = Money.ToMinor(money.Amount, targetCurrency);

        return minorResult.IsFailure
            ? Fail<long>(minorResult.Error)
            : Ok(minorResult.Value);
    }
}
