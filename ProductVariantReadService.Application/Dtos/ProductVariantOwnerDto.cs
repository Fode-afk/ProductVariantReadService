namespace ProductVariantReadService.Application.Dtos;

public sealed record ProductVariantOwnerDto(
    Guid VariantId,
    Guid ProductId,
    Guid VendorId,
    string Sku,
    DimensionsInfoDto Dimensions,
    WeightInfoDto Weight,
    string Barcode,
    PriceInfoDto? Price,
    StockInfoDto? Stock,
    ReviewSummaryDto? ReviewSummary,
    List<ProductVariantAttributeDto> Attributes,
    List<VariantImageDto> Images,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
