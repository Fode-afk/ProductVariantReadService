using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductVariantReadService.Domain.Snapshots;

public sealed class ProductSnapshot
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid ProductId { get; set; }

    public bool IsVisiblePublicly { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; }
}