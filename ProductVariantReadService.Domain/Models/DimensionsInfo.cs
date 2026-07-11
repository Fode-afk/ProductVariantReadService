namespace ProductVariantReadService.Domain.Models;

public sealed class DimensionsInfo
{
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public required string Unit { get; set; }
}
