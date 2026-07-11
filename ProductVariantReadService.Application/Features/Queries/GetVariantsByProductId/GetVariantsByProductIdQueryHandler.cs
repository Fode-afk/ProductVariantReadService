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
using System.Reflection.Metadata;
using ZiggyCreatures.Caching.Fusion;
using static migApp.Shared.Results.ResultFactory;

namespace ProductVariantReadService.Application.Features.Queries.GetVariantsByProductId;

public sealed class GetVariantsByProductIdQueryHandler(
    IProductVariantReadRepository productReadRepository,
    IMoneyConverter moneyConverter,
    IProductVariantReadMetrics metrics,
    IFusionCache cache) : IRequestHandler<GetVariantsByProductIdQuery, IResult<IEnumerable<ProductVariantPublicDto>>>
{
    public async Task<IResult<IEnumerable<ProductVariantPublicDto>>> Handle(GetVariantsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var currencyResult = Currency.Create(request.Currency);
        if (currencyResult.IsFailure)
            return Fail<IEnumerable<ProductVariantPublicDto>>(currencyResult.Error);

        var currency = currencyResult.Value;

        var wasHit = true;

        var variantDtos = await cache.GetOrSetAsync<IEnumerable<ProductVariantPublicDto>?>(
            CacheKeys.VariantsByProductId(request.ProductId, currency.Code),
            async (entry, ct) =>
            {
                wasHit = false;

                var variants = await productReadRepository.GetByProductIdAsync(request.ProductId, ct);

                var visibleVariants = variants?
                    .Where(v => v.Product.IsVisiblePublicly)
                    .ToList() ?? [];

                if (visibleVariants.Count == 0)
                {
                    metrics.RecordProductVariantHiddenByVisibilityPolicy("Some variants are hidden by visibility policy");
                    return null;
                }

                var dtos = new List<ProductVariantPublicDto>(visibleVariants.Count);

                foreach (var variant in visibleVariants)
                {
                    var dtoResult = await ToConvertedPublicDtoAsync(variant, currency, ct);

                    if (dtoResult.IsFailure)
                        return null;

                    dtos.Add(dtoResult.Value);
                }

                return dtos;
            },
            tags: [CacheTags.VariantsByProductId(request.ProductId)],
            token: cancellationToken);

        metrics.RecordCacheHitOrMiss("product-variant-by-product-id", wasHit);

        return variantDtos != null && variantDtos.Any() ?
            Ok(variantDtos) :
            Fail<IEnumerable<ProductVariantPublicDto>>(ProductVariantErrors.NotFound());
    }

    private async Task<IResult<ProductVariantPublicDto>> ToConvertedPublicDtoAsync(
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
                return Fail<ProductVariantPublicDto>(amountResult.Error);

            long? oldAmountMinor = null;

            if (document.Price.OldAmountMinor is not null)
            {
                var oldAmountResult = await ConvertUsdToTargetMinorAsync(
                    document.Price.OldAmountMinor.Value, targetCurrency, cancellationToken);

                if (oldAmountResult.IsFailure)
                    return Fail<ProductVariantPublicDto>(oldAmountResult.Error);

                oldAmountMinor = oldAmountResult.Value;
            }

            priceDto = new PriceInfoDto(amountResult.Value, oldAmountMinor);
        }

        return Ok(document.ToPublicDto() with { Price = priceDto });
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