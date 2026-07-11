using MediatR;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantPriceInfo;

public sealed record UpdateProductVariantPriceInfoCommand(
    Guid VariantId,
    Guid ProductId,
    long PriceMinor,
    long? OldPriceMinor) : IRequest;