using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using ProductVariantReadService.Infrastructure.Data;

namespace ProductVariantReadService.Infrastructure.DependencyInjection;

public static class HostExtensions
{
    public static async Task MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

        await MongoIndexInitializer.EnsureIndexesAsync(database);
    }
}