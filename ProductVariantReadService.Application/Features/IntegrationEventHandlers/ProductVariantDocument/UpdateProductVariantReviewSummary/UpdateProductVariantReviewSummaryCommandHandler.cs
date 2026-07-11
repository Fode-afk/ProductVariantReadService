using MediatR;
using ProductVariantReadService.Application.Caching;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Application.Interfaces.Metrics;
using ProductVariantReadService.Domain.Models;
using ZiggyCreatures.Caching.Fusion;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantReviewSummary;

public sealed class UpdateProductVariantReviewSummaryCommandHandler(
    IProductVariantReadRepository repository,
    IFusionCache cache,
    IProductVariantReadMetrics metrics,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductVariantReviewSummaryCommand>
{
    public async Task Handle(UpdateProductVariantReviewSummaryCommand request, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();

        var reviewSummary = new ReviewSummary
        {
            AverageRating = request.AverageRating,
            TotalCount = request.TotalCount,
            UpdatedAt = now
        };

        await repository.UpdateReviewSummaryAsync(
            request.VariantId,
            reviewSummary,
            now,
            cancellationToken);

        await cache.RemoveByTagAsync(CacheTags.VariantsByProductId(request.ProductId), token: cancellationToken);
        metrics.RecordCacheInvalidation("variants-by-product-id");
    }
}
