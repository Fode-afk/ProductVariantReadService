using MongoDB.Driver;
using ProductVariantReadService.Domain.Models;

namespace ProductVariantReadService.Infrastructure.Data;

internal static class MongoIndexInitializer
{
    public static async Task EnsureIndexesAsync(
        IMongoDatabase database,
        CancellationToken cancellationToken = default)
    {
        var indexes = new List<CreateIndexModel<ProductVariantDocument>>
        {
            new(Builders<ProductVariantDocument>.IndexKeys
                    .Ascending(v => v.Sku),
                new CreateIndexOptions { Unique = true, Name = "sku_unique" }),

            new(Builders<ProductVariantDocument>.IndexKeys
                    .Ascending(v => v.Product.ProductId),
                new CreateIndexOptions { Name = "product_id" }),

            new(Builders<ProductVariantDocument>.IndexKeys
                    .Ascending(v => v.Product.ProductId)
                    .Ascending(v => v.Product.IsVisiblePublicly),
                new CreateIndexOptions { Name = "product_id_visibility" }),

            new(Builders<ProductVariantDocument>.IndexKeys
                    .Ascending(v => v.Product.ProductId)
                    .Ascending("stock.inStock"),
                new CreateIndexOptions { Name = "product_id_in_stock" }),

            new(Builders<ProductVariantDocument>.IndexKeys
                    .Ascending(v => v.Barcode),
                new CreateIndexOptions { Name = "barcode" }),

            new(Builders<ProductVariantDocument>.IndexKeys
                    .Descending(v => v.CreatedAt),
                new CreateIndexOptions { Name = "created_at_desc" }),
        };

        var variants = database.GetCollection<ProductVariantDocument>("productVariantDocuments");
        await variants.Indexes.CreateManyAsync(indexes, cancellationToken);
    }
}