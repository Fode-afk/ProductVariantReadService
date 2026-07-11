using MediatR;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.DeleteProductVariant;

public sealed record DeleteProductVariantCommand(Guid VariantId, Guid ProductId) : IRequest;