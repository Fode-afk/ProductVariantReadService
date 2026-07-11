using MediatR;
using ZiggyCreatures.Caching.Fusion;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Domain.Exceptions;
using ProductVariantReadService.Application.Interfaces.Metrics;
using ProductVariantReadService.Domain.Models;
using ProductVariantReadService.Application.Caching;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantImages;

public sealed class UpdateProductVariantImagesCommandHandler(
    IProductVariantReadRepository repository,
    IFusionCache cache,
    IProductVariantReadMetrics metrics,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductVariantImagesCommand>
{
    public async Task Handle(UpdateProductVariantImagesCommand request, CancellationToken cancellationToken)
    {
        var images = request.Images.Select(i =>
            new VariantImage
            {
                Url = i.Url,
                Alt = i.Alt,
                SortOrder = i.SortOrder,
                IsMain = i.IsMain,
            }).ToList();

        var applied = await repository.TryUpdateImagesAsync(
            request.VariantId,
            images,
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

            metrics.RecordProductVariantOutdated(nameof(UpdateProductVariantImagesCommandHandler));
            return;
        }

        await cache.RemoveByTagAsync(CacheTags.VariantsByProductId(request.ProductId), token: cancellationToken);
        metrics.RecordCacheInvalidation("variants-by-product-id");
    }
}
