namespace ProductVariantReadService.Application.Dtos;

public sealed record PriceInfoDto(
    long AmountMinor,
    long? OldAmountMinor);
