using MediatR;
using migApp.Shared.Dtos.ProductVariant;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpsertProductVariantDocument;

public sealed record UpsertProductVariantDocumentCommand(
    Guid VariantId,
    Guid ProductId,
    string Sku,
    DimensionsDto Dimensions,
    WeightDto Weight,
    string Barcode,
    List<VariantAttributeDto> Attributes,
    long Version) : IRequest;