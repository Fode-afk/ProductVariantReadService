using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;
using ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductSuspendedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductSuspendedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductSuspendedIntegrationEvent> context) =>
        await mediator.Send(new UpdateProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.VendorId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
