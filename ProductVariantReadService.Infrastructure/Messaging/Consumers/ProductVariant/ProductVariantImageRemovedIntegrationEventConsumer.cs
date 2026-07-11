using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.ProductVariants;
using ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpdateProductVariantImages;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.ProductVariant;

public sealed class ProductVariantImageRemovedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductVariantImageRemovedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductVariantImageRemovedIntegrationEvent> context) => 
        await mediator.Send(new UpdateProductVariantImagesCommand(
            context.Message.ProductVariantId,
            context.Message.ProductId,
            context.Message.Images,
            context.Message.Version), context.CancellationToken);
}
