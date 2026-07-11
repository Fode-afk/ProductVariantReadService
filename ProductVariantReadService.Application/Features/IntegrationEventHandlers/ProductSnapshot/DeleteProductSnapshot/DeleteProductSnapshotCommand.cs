using MediatR;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.DeleteProductSnapshot;

public sealed record DeleteProductSnapshotCommand(Guid ProductId) : IRequest;