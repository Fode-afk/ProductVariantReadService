using MediatR;
using ProductVariantReadService.Application.Interfaces.Metrics;
using ProductVariantReadService.Application.Interfaces.Data;
using ProductVariantReadService.Domain.Exceptions;
using ProductVariantReadService.Domain.Models;

namespace ProductVariantReadService.Application.Features.IntegrationEventHandlers.ProductVariantDocument.UpsertProductVariantDocument;

public sealed class UpsertProductVariantDocumentCommandHandler(
    IProductVariantReadRepository productvariantRepository,
    IProductSnapshotRepository productSnapshotRepository,
    IProductVariantReadMetrics metrics,
    TimeProvider timeProvider) : IRequestHandler<UpsertProductVariantDocumentCommand>
{
    public async Task Handle(UpsertProductVariantDocumentCommand request, CancellationToken cancellationToken)
    {
        var productSnapshot = await productSnapshotRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (productSnapshot is null)
        {
            metrics.RecordSnapshotNotFound("ProductSnapshot");
            throw new SnapshotNotFoundException("ProductSnapshot", request.ProductId);
        }

        var productVariantDocument = new Domain.Models.ProductVariantDocument
        {
            VariantId = request.VariantId,
            Product = new ProductInfo
            {
                ProductId = request.ProductId,
                IsVisiblePublicly = productSnapshot.IsVisiblePublicly,
            },
            Sku = request.Sku,
            Dimensions = new DimensionsInfo
            {
                Length = request.Dimensions.Length,
                Width = request.Dimensions.Width,
                Height = request.Dimensions.Height,
                Unit = request.Dimensions.Unit
            },
            Weight = new WeightInfo
            {
                Value = request.Weight.Value,
                Unit = request.Weight.Unit
            },
            Barcode = request.Barcode,
            Attributes = [.. request.Attributes.Select(a => new ProductVariantAttribute
            {
                CharacteristicId = a.CharacteristicId,
                Name = a.Name,
                Value = a.Value,
                CharType = a.CharType.ToString(),
                GroupName = a.GroupName,
            })],
            CreatedAt = timeProvider.GetUtcNow(),
            Version = request.Version,
        };

        await productvariantRepository.UpsertAsync(productVariantDocument, cancellationToken);
    }
}
