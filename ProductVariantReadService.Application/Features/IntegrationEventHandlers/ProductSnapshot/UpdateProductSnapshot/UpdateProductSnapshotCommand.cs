using MediatR;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;

public sealed record UpdateProductSnapshotCommand(
    Guid ProductId,
    Guid VendorId,
    bool IsVisiblePublicly,
    long Version) : IRequest;