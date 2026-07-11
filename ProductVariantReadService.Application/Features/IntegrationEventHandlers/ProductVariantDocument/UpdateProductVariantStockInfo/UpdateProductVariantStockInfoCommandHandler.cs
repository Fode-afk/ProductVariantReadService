using MediatR;
using ProductVariantReadService.Application.Caching;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Application.Interfaces.Metrics;
using ProductVariantReadService.Domain.Models;
using ZiggyCreatures.Caching.Fusion;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantStockInfo;

public sealed class UpdateProductVariantStockInfoCommandHandler(
    IProductVariantReadRepository repository,
    IFusionCache cache,
    IProductVariantReadMetrics metrics,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductVariantStockInfoCommand>
{
    public async Task Handle(UpdateProductVariantStockInfoCommand request, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();

        var stock = new StockInfo
        {
            InStock = request.InStock,
            UpdatedAt = now
        };

        await repository.UpdateStockInfoAsync(
            request.VariantId,
            stock,
            now,
            cancellationToken);

        await cache.RemoveByTagAsync(CacheTags.VariantsByProductId(request.ProductId), token: cancellationToken);
        metrics.RecordCacheInvalidation("variants-by-product-id");
    }
}
