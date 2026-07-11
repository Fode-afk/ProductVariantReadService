namespace ProductVariantReadService.Application.Dtos;

public sealed record VariantImageDto(
    string Url,
    string Alt,
    int SortOrder,
    bool IsMain);