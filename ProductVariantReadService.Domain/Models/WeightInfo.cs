namespace ProductVariantReadService.Domain.Models;

public sealed class WeightInfo
{
    public double Value { get; set; }
    public required string Unit { get; set; }
}
