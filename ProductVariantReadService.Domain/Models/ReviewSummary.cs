namespace ProductVariantReadService.Domain.Models;

public sealed class ReviewSummary
{
    public decimal AverageRating { get; set; }
    public int TotalCount { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}