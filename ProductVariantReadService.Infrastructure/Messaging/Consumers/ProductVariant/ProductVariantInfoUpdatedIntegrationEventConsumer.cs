using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.ProductVariants;
using ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantInfo;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.ProductVariant;

public sealed class ProductVariantInfoUpdatedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductVariantInfoUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductVariantInfoUpdatedIntegrationEvent> context)
    {
        var message = context.Message;

        await mediator.Send(new UpdateProductVariantInfoCommand(
            message.ProductVariantId,
            message.ProductId,
            message.Sku,
            message.Dimensions,
            message.Weight,
            message.Barcode,
            message.Version), context.CancellationToken);
    }
}
