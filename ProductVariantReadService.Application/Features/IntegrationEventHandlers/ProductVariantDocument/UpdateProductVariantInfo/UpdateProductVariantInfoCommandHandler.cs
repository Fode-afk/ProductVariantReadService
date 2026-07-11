using MediatR;
using ProductVariantReadService.Application.Caching;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Application.Interfaces.Metrics;
using ProductVariantReadService.Domain.Exceptions;
using ProductVariantReadService.Domain.Models;
using ZiggyCreatures.Caching.Fusion;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantInfo;

public sealed class UpdateProductVariantInfoCommandHandler(
    IProductVariantReadRepository repository,
    IFusionCache cache,
    IProductVariantReadMetrics metrics,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductVariantInfoCommand>
{
    public async Task Handle(UpdateProductVariantInfoCommand request, CancellationToken cancellationToken)
    {
        var dimensions = new DimensionsInfo
        {
            Length = request.Dimensions.Length,
            Width = request.Dimensions.Width,
            Height = request.Dimensions.Height,
            Unit = request.Dimensions.Unit
        };

        var weight = new WeightInfo
        {
            Value = request.Weight.Value,
            Unit = request.Weight.Unit
        };

        var applied = await repository.TryUpdateInfoAsync(
            request.VariantId,
            request.Sku,
            dimensions,
            weight,
            request.Barcode,
            request.Version,
            timeProvider.GetUtcNow(),
            cancellationToken);

        if (!applied)
        {
            var existing = await repository.GetByIdAsync(request.VariantId, cancellationToken);

            if (existing is null)
            {
                metrics.RecordProductVariantNotFound();
                throw new ProductVariantNotFoundException(request.VariantId);
            }

            metrics.RecordProductVariantOutdated(nameof(UpdateProductVariantInfoCommandHandler));
            return;
        }

        await cache.RemoveByTagAsync(CacheTags.VariantsByProductId(request.ProductId), token: cancellationToken);
        metrics.RecordCacheInvalidation("variants-by-product-id");
    }
}
