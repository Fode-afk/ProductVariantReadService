namespace ProductVariantReadService.Domain.Models;

public sealed class PriceInfo
{
    public long AmountMinor { get; set; }
    public long? OldAmountMinor { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
