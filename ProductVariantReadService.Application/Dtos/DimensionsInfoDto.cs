namespace ProductVariantReadService.Application.Dtos;

public sealed record DimensionsInfoDto(
    double Length,
    double Width,
    double Height,
    string Unit);
