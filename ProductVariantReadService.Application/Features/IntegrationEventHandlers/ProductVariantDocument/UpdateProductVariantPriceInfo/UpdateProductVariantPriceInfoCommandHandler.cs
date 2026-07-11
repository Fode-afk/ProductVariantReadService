using MediatR;
using ProductVariantReadService.Application.Caching;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Application.Interfaces.Metrics;
using ProductVariantReadService.Domain.Models;
using ZiggyCreatures.Caching.Fusion;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantPriceInfo;

public sealed class UpdateProductVariantPriceInfoCommandHandler(
    IProductVariantReadRepository repository,
    IFusionCache cache,
    IProductVariantReadMetrics metrics,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductVariantPriceInfoCommand>
{
    public async Task Handle(UpdateProductVariantPriceInfoCommand request, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();

        var price = new PriceInfo
        { 
            AmountMinor = request.PriceMinor,
            OldAmountMinor = request.OldPriceMinor,
            UpdatedAt = now
        };

        await repository.UpdatePriceInfoAsync(
            request.VariantId,
            price,
            now,
            cancellationToken);

        await cache.RemoveByTagAsync(CacheTags.VariantsByProductId(request.ProductId), token: cancellationToken);
        metrics.RecordCacheInvalidation("variants-by-product-id");
    }
}
