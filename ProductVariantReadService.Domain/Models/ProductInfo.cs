using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductVariantReadService.Domain.Models;

public sealed class ProductInfo
{
    [BsonRepresentation(BsonType.String)]
    public Guid ProductId { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid VendorId { get; set; }
    public bool IsVisiblePublicly { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
