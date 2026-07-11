using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.ProductVariants;
using ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.DeleteProductVariant;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.ProductVariant;

public sealed class ProductVariantForceDeletedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductVariantForceDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductVariantForceDeletedIntegrationEvent> context) => 
        await mediator.Send(new DeleteProductVariantCommand(
            context.Message.ProductVariantId,
            context.Message.ProductId), context.CancellationToken);
}
