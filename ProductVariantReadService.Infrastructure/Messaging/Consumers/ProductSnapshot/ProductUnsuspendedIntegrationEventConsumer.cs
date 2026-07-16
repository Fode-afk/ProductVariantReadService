using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;
using ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.ProductSnapshot;

public sealed class ProductUnsuspendedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductUnsuspendedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductUnsuspendedIntegrationEvent> context) =>
        await mediator.Send(new UpdateProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.VendorId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
