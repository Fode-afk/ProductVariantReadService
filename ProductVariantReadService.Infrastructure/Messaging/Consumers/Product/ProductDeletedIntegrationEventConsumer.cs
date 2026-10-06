using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;
using ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.DeleteProductSnapshot;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductDeletedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedIntegrationEvent> context) =>
        await mediator.Send(new DeleteProductSnapshotCommand(context.Message.ProductId), context.CancellationToken);
}