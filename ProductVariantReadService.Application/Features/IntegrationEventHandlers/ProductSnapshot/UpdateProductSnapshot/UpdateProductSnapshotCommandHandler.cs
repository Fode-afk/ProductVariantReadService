using MediatR;
using ZiggyCreatures.Caching.Fusion;
using ProductVariantReadService.Application.Interfaces.Metrics;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Domain.Exceptions;
using ProductVariantReadService.Domain.Models;
using ProductVariantReadService.Application.Caching;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;

public sealed class UpdateProductSnapshotCommandHandler(
    IProductSnapshotRepository productSnapshotRepository,
    IProductVariantReadRepository productVariantReadRepository,
    IProductVariantReadMetrics metrics,
    IFusionCache cache,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductSnapshotCommand>
{
    public async Task Handle(UpdateProductSnapshotCommand request, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();

        var applied = await productSnapshotRepository.TryUpdateAsync(
            request.ProductId,
            request.IsVisiblePublicly,
            request.Version,
            now,
            cancellationToken);

        if (!applied)
        {
            var existing = await productSnapshotRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (existing is null)
            {
                metrics.RecordSnapshotNotFound("Product");
                throw new SnapshotNotFoundException("Product", request.ProductId);
            }

            metrics.RecordSnapshotOutdated("Product");
            return;
        }

        var productInfo = new ProductInfo
        {
            ProductId = request.ProductId,
            IsVisiblePublicly = request.IsVisiblePublicly,
            UpdatedAt = now,
        };

        await productVariantReadRepository.UpdateProductInfoForAllVariantsAsync(
            productInfo,
            now,
            cancellationToken);

        await cache.RemoveByTagAsync(CacheTags.VariantsByProductId(request.ProductId), token: cancellationToken);
        metrics.RecordCacheInvalidation("variants-by-product-id");
    }
}
