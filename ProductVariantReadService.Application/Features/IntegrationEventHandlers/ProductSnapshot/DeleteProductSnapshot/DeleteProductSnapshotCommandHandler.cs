using MediatR;
using ProductVariantReadService.Application.Interfaces.Data;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.DeleteProductSnapshot;

public sealed class DeleteProductSnapshotCommandHandler(IProductSnapshotRepository productSnapshotRepository) : IRequestHandler<DeleteProductSnapshotCommand>
{
    public async Task Handle(DeleteProductSnapshotCommand request, CancellationToken cancellationToken) =>
        await productSnapshotRepository.DeleteAsync(request.ProductId, cancellationToken);
}
