using MediatR;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantReviewSummary;

public sealed record UpdateProductVariantReviewSummaryCommand(
    Guid VariantId,
    Guid ProductId,
    decimal AverageRating,
    int TotalCount) : IRequest;