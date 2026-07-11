using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;
using ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.AddProductSnapshot;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.ProductSnapshot;

public sealed class ProductCreatedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context) =>
        await mediator.Send(new AddProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
