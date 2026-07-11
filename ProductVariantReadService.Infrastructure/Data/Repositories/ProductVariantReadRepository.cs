using MongoDB.Driver;
using ProductVariantReadService.Domain.Models;
using ProductVariantReadService.Application.Interfaces.Data;

namespace ProductVariantReadService.Infrastructure.Data.Repositories;

internal sealed class ProductVariantReadRepository(IMongoDatabase database) : IProductVariantReadRepository
{
    private readonly IMongoCollection<ProductVariantDocument> _collection =
        database.GetCollection<ProductVariantDocument>("productVariantDocuments");

    public async Task<ProductVariantDocument?> GetByIdAsync(
        Guid variantId,
        CancellationToken cancellationToken = default) =>
        await _collection.Find(v => v.VariantId == variantId).FirstOrDefaultAsync(cancellationToken);

    public async Task<ProductVariantDocument?> GetBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default) =>
        await _collection.Find(v => v.Sku == sku).FirstOrDefaultAsync(cancellationToken);
    public async Task<IEnumerable<ProductVariantDocument>> GetByProductIdAsync(
        Guid productId, 
        CancellationToken cancellationToken = default) =>
        await _collection.Find(v => v.Product.ProductId == productId).ToListAsync(cancellationToken);

    public async Task UpsertAsync(
        ProductVariantDocument document,
        CancellationToken cancellationToken = default) =>
        await _collection.ReplaceOneAsync(
            v => v.VariantId == document.VariantId,
            document,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);

    public async Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        (int)await _collection.CountDocumentsAsync(
            FilterDefinition<ProductVariantDocument>.Empty,
            cancellationToken: cancellationToken);

    public async Task<bool> TryUpdateInfoAsync(
        Guid variantId,
        string sku,
        DimensionsInfo dimensions,
        WeightInfo weight,
        string barcode,
        long version,
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        var result = await _collection.UpdateOneAsync(
            v => v.VariantId == variantId && v.Version < version,
            Builders<ProductVariantDocument>.Update
                .Set(v => v.Sku, sku)
                .Set(v => v.Dimensions, dimensions)
                .Set(v => v.Weight, weight)
                .Set(v => v.Barcode, barcode)
                .Set(v => v.Version, version)
                .Set(v => v.UpdatedAt, now),
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> TryUpdateImagesAsync(
        Guid variantId,
        List<VariantImage> images,
        long version,
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        var result = await _collection.UpdateOneAsync(
            v => v.VariantId == variantId && v.Version < version,
            Builders<ProductVariantDocument>.Update
                .Set(v => v.Images, images)
                .Set(v => v.Version, version)
                .Set(v => v.UpdatedAt, now),
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    public async Task UpdatePriceInfoAsync(
        Guid variantId,
        PriceInfo price,
        DateTimeOffset now,
        CancellationToken cancellationToken = default) =>
        await _collection.UpdateOneAsync(
            v => v.VariantId == variantId,
            Builders<ProductVariantDocument>.Update
                .Set(v => v.Price, price)
                .Set(v => v.UpdatedAt, now),
            cancellationToken: cancellationToken);

    public async Task UpdateStockInfoAsync(
        Guid variantId,
        StockInfo stock,
        DateTimeOffset now,
        CancellationToken cancellationToken = default) => 
        await _collection.UpdateOneAsync(
            v => v.VariantId == variantId,
            Builders<ProductVariantDocument>.Update
                .Set(v => v.Stock, stock)
                .Set(v => v.UpdatedAt, now),
            cancellationToken: cancellationToken); 

    public async Task UpdateReviewSummaryAsync(
        Guid variantId,
        ReviewSummary reviewSummary,
        DateTimeOffset now,
        CancellationToken cancellationToken = default) =>
        await _collection.UpdateOneAsync(
            v => v.VariantId == variantId,
            Builders<ProductVariantDocument>.Update
                .Set(v => v.ReviewSummary, reviewSummary)
                .Set(v => v.UpdatedAt, now),
            cancellationToken: cancellationToken);

    public async Task UpdateProductInfoForAllVariantsAsync(
        ProductInfo product,
        DateTimeOffset now,
        CancellationToken cancellationToken = default) =>
        await _collection.UpdateManyAsync(
            v => v.Product.ProductId == product.ProductId,
            Builders<ProductVariantDocument>.Update
                .Set(v => v.Product, product)
                .Set(v => v.UpdatedAt, now),
            cancellationToken: cancellationToken);

    public async Task DeleteAsync(Guid variantId, CancellationToken cancellationToken = default) =>
        await _collection.DeleteOneAsync(v => v.VariantId == variantId, cancellationToken);
}