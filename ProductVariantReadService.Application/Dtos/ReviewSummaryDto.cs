namespace ProductVariantReadService.Application.Dtos;

public sealed record ReviewSummaryDto(
    decimal AverageRating,
    int TotalCount);
