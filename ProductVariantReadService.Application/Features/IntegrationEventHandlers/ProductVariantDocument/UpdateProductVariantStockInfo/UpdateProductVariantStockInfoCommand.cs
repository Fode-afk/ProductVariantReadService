using MediatR;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantStockInfo;

public sealed record UpdateProductVariantStockInfoCommand(
    Guid VariantId,
    Guid ProductId,
    bool InStock) : IRequest;
