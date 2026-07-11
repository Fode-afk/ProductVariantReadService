using MongoDB.Driver;
using ProductVariantReadService.Domain.Snapshots;
using ProductVariantReadService.Application.Interfaces.Data;

namespace ProductVariantReadService.Infrastructure.Data.Repositories;

internal sealed class ProductSnapshotRepository(IMongoDatabase database) : IProductSnapshotRepository
{
    private readonly IMongoCollection<ProductSnapshot> _collection = database.GetCollection<ProductSnapshot>("productSnapshots");

    public async Task<ProductSnapshot?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default) =>
       await _collection
           .Find(s => s.ProductId == productId)
           .FirstOrDefaultAsync(cancellationToken);

    public Task AddAsync(ProductSnapshot snapshot, CancellationToken cancellationToken = default) =>
        _collection.InsertOneAsync(snapshot, cancellationToken: cancellationToken);

    public async Task<bool> TryUpdateAsync(
        Guid productId,
        bool isVisiblePublicly,
        long version,
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        var result = await _collection.UpdateOneAsync(
            s => s.ProductId == productId && s.Version < version,
            Builders<ProductSnapshot>.Update
                .Set(s => s.IsVisiblePublicly, isVisiblePublicly)
                .Set(s => s.Version, version)
                .Set(s => s.UpdatedAt, now),
            cancellationToken: cancellationToken);

        return result.MatchedCount > 0;
    }

    public async Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default) =>
        await _collection.DeleteOneAsync(s => s.ProductId == productId, cancellationToken);
}
