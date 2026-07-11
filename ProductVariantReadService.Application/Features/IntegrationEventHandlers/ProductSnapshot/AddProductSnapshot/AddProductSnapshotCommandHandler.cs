using MediatR;
using ProductVariantReadService.Application.Interfaces.Data;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.AddProductSnapshot;

public sealed class AddProductSnapshotCommandHandler(
    IProductSnapshotRepository productSnapshotRepository,
    TimeProvider timeProvider) : IRequestHandler<AddProductSnapshotCommand>
{
    public async Task Handle(AddProductSnapshotCommand request, CancellationToken cancellationToken)
    {
        var productSnapshot = new Domain.Snapshots.ProductSnapshot
        {
            ProductId = request.ProductId,
            IsVisiblePublicly = request.IsVisiblePublicly,
            Version = request.Version,
            UpdatedAt = timeProvider.GetUtcNow()
        };

        await productSnapshotRepository.AddAsync(productSnapshot, cancellationToken);
    }
}
