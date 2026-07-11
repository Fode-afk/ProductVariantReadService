using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductVariantReadService.Domain.Models;

public sealed class ProductVariantAttribute
{
    [BsonRepresentation(BsonType.String)]
    public Guid CharacteristicId { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
    public required string CharType { get; set; }
    public string? GroupName { get; set; }
}