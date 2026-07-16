using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.ExchangeRates;
using ProductVariantReadService.Application.Interfaces.Services;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.Currencies;

public sealed class CurrenciesUpdatedIntegrationEventConsumer(IExchangeRateService exchangeRateService) : IConsumer<CurrenciesUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<CurrenciesUpdatedIntegrationEvent> context) =>
       await exchangeRateService.UpdateExcahngeRatesAsync(context.Message.Rates, context.CancellationToken);
}