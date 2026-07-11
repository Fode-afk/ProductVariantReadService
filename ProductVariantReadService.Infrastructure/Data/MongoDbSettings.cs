namespace ProductVariantReadService.Infrastructure.Data;

public sealed class MongoDbSettings
{
    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
}
