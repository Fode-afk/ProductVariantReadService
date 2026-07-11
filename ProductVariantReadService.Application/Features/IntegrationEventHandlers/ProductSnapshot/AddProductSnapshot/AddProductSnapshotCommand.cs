using MediatR;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.AddProductSnapshot;

public sealed record AddProductSnapshotCommand(
    Guid ProductId,
    bool IsVisiblePublicly,
    long Version) : IRequest;