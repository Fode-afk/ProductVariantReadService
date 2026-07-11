using ProductVariantReadService.Domain.Models;

namespace ProductVariantReadService.Application.Interfaces.Data;

public interface IProductVariantReadRepository
{
    Task<ProductVariantDocument?> GetByIdAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task<ProductVariantDocument?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductVariantDocument>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task UpsertAsync(ProductVariantDocument document, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<bool> TryUpdateInfoAsync(
        Guid variantId,
        string Sku,
        DimensionsInfo Dimensions,
        WeightInfo Weight,
        string Barcode,
        long version,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task<bool> TryUpdateImagesAsync(
        Guid variantId,
        List<VariantImage> images,
        long version,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task UpdatePriceInfoAsync(
        Guid variantId,
        PriceInfo price,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task UpdateStockInfoAsync(
        Guid variantId,
        StockInfo stock,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task UpdateReviewSummaryAsync(
        Guid variantId,
        ReviewSummary reviewSummary,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task UpdateProductInfoForAllVariantsAsync(
        ProductInfo product,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid variantId, CancellationToken cancellationToken = default);
}