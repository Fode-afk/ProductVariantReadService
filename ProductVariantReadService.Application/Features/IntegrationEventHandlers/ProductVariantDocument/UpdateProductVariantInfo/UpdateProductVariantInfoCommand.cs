using MediatR;
using migApp.Shared.Dtos.ProductVariant;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantInfo;

public sealed record UpdateProductVariantInfoCommand(
    Guid VariantId,
    Guid ProductId,
    string Sku,
    DimensionsDto Dimensions,
    WeightDto Weight,
    string Barcode,
    long Version) : IRequest;
