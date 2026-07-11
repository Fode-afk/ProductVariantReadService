namespace ProductVariantReadService.Application.Dtos;

public sealed record ProductVariantPublicDto(
    Guid VariantId,
    Guid ProductId,
    PriceInfoDto? Price,
    StockInfoDto? Stock,
    ReviewSummaryDto? ReviewSummary,
    List<ProductVariantAttributeDto> Attributes,
    List<VariantImageDto> Images);