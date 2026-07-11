namespace ProductVariantReadService.Application.Dtos;

public sealed record ProductVariantAttributeDto(
    Guid CharacteristicId,
    string Name,
    string Value,
    string CharType,
    string? GroupName);
