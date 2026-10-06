using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;
using ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;

namespace ProductVariantReadService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductPublishSubmissionWithdrawnIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductPublishSubmissionWithdrawnIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPublishSubmissionWithdrawnIntegrationEvent> context) =>
        await mediator.Send(new UpdateProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.VendorId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
