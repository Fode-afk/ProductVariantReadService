using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductVariantReadService.Domain.Models;

public sealed class ProductVariantDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid VariantId { get; set; }
    public ProductInfo Product { get; set; } = null!;

    public required string Sku { get; set; }
    public DimensionsInfo Dimensions { get; set; } = null!;
    public WeightInfo Weight { get; set; } = null!;
    public required string Barcode { get; set; }

    public PriceInfo? Price { get; set; }
    public StockInfo? Stock { get; set; }

    public ReviewSummary? ReviewSummary { get; set; }

    public List<ProductVariantAttribute> Attributes { get; set; } = [];
    public List<VariantImage> Images { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public long Version { get; set; }
}
