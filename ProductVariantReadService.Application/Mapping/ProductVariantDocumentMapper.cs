using ProductVariantReadService.Application.Dtos;
using ProductVariantReadService.Domain.Models;

namespace ProductVariantReadService.Application.Mapping;

internal static class ProductVariantDocumentMapper
{
    public static ProductVariantPublicDto ToPublicDto(this ProductVariantDocument document) =>
        new(
            document.VariantId,
            document.Product.ProductId,
            document.Product.VendorId,  
            MapPriceInfo(document.Price),
            MapStockInfo(document.Stock),
            MapReviewSummary(document.ReviewSummary),
            [.. document.Attributes.Select(MapAttribute)],
            [.. document.Images.Select(MapImage)]);

    public static ProductVariantOwnerDto ToOwnerDto(this ProductVariantDocument document) =>
        new(
            document.VariantId,
            document.Product.ProductId,
            document.Product.VendorId,
            document.Sku,
            MapDimensions(document.Dimensions),
            MapWeight(document.Weight),
            document.Barcode,
            MapPriceInfo(document.Price),
            MapStockInfo(document.Stock),
            MapReviewSummary(document.ReviewSummary),
            [.. document.Attributes.Select(MapAttribute)],
            [.. document.Images.Select(MapImage)],
            document.CreatedAt,
            document.UpdatedAt);

    private static PriceInfoDto? MapPriceInfo(PriceInfo? price) =>
        price is null ? null : new PriceInfoDto(price.AmountMinor, price.OldAmountMinor);

    private static StockInfoDto? MapStockInfo(StockInfo? stock) =>
        stock is null ? null : new StockInfoDto(stock.InStock);

    private static DimensionsInfoDto MapDimensions(DimensionsInfo dimensions) =>
        new(dimensions.Length, dimensions.Width, dimensions.Height, dimensions.Unit);

    private static WeightInfoDto MapWeight(WeightInfo weight) =>
        new(weight.Value, weight.Unit);

    private static ReviewSummaryDto? MapReviewSummary(ReviewSummary? summary) =>
        summary is null ? null : new ReviewSummaryDto(summary.AverageRating, summary.TotalCount);

    private static ProductVariantAttributeDto MapAttribute(ProductVariantAttribute attribute) =>
        new(
            attribute.CharacteristicId,
            attribute.Name,
            attribute.Value, 
            attribute.CharType,
            attribute.GroupName);

    private static VariantImageDto MapImage(VariantImage image) =>
        new(
            image.Url,
            image.Alt,
            image.SortOrder,
            image.IsMain);
}