namespace ProductVariantReadService.Domain.Models;

public sealed class StockInfo
{
    public bool InStock { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
