using MediatR;
using migApp.Shared.Dtos.ProductVariant;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantImages;

public sealed record UpdateProductVariantImagesCommand(
    Guid VariantId,
    Guid ProductId,
    List<ProductVariantImageDto> Images,
    long Version) : IRequest;