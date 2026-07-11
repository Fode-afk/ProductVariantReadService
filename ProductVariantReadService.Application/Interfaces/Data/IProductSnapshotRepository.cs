using ProductVariantReadService.Domain.Snapshots;

namespace ProductVariantReadService.Application.Interfaces.Data;

public interface IProductSnapshotRepository
{
    Task<ProductSnapshot?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task AddAsync(ProductSnapshot snapshot, CancellationToken cancellationToken = default);

    Task<bool> TryUpdateAsync(
        Guid productId,
        bool isVisiblePublicly,
        long version,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default);
}