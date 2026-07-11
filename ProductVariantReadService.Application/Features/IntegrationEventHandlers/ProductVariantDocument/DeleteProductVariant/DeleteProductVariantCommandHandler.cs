using MediatR;
using ProductVariantReadService.Application.Caching;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Application.Interfaces.Metrics;
using ZiggyCreatures.Caching.Fusion;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.DeleteProductVariant;

public sealed class DeleteProductVariantCommandHandler(
    IProductVariantReadRepository repository,
    IProductVariantReadMetrics metrics,
    IFusionCache cache) : IRequestHandler<DeleteProductVariantCommand>
{
    public async Task Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.VariantId, cancellationToken);

        await cache.RemoveByTagAsync(CacheTags.VariantsByProductId(request.ProductId), token: cancellationToken);
        metrics.RecordCacheInvalidation("variants-by-product-id");
    }
}
