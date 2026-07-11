using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Application.Interfaces.Metrics;

namespace ProductVariantReadService.Infrastructure.Observability;

internal sealed class IndexedProductVariantsCountCollector(
    IServiceScopeFactory scopeFactory,
    ILogger<IndexedProductVariantsCountCollector> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IProductVariantReadRepository>();
                var metrics = scope.ServiceProvider.GetRequiredService<IProductVariantReadMetrics>();

                var count = await repository.CountAsync(stoppingToken);
                metrics.SetIndexedProductVariantsCount(count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update indexed product variants count metric.");
            }
        }
    }
}
