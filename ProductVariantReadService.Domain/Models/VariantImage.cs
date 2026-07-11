namespace ProductVariantReadService.Domain.Models;

public sealed class VariantImage
{
    public required string Url { get; set; }
    public required string Alt { get; set; }
    public int SortOrder { get; set; }
    public bool IsMain { get; set; }
}
