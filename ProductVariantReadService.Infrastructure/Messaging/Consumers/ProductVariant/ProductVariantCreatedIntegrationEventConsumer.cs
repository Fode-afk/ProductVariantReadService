using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.ProductVariants;
using ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpsertProductVariantDocument;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.ProductVariant;

public sealed class ProductVariantCreatedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductVariantCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductVariantCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        await mediator.Send(new UpsertProductVariantDocumentCommand(
            message.ProductVariantId,
            message.ProductId,
            message.Sku,
            message.Dimensions,
            message.Weight,
            message.Barcode,
            message.Attributes,
            message.Version), context.CancellationToken);
    }
}