using ProductVariantReadService.Api.Grpc.V1.Protos;
using ProductVariantReadService.Application.Dtos;
using Proto = ProductVariantReadService.Api.Grpc.V1.Protos;

namespace ProductVariantReadService.Api.Grpc.Mapping;

internal static class VariantDtoMapper
{
    public static VariantPublicDto ToGrpc(this ProductVariantPublicDto dto)
    {
        var proto = new VariantPublicDto
        {
            VariantId = dto.VariantId.ToString(),
            ProductId = dto.ProductId.ToString(),
            Price = dto.Price?.ToGrpc(),
            Stock = dto.Stock?.ToGrpc(),
            ReviewSummary = dto.ReviewSummary?.ToGrpc(),
            Attributes = { dto.Attributes.Select(a => a.ToGrpc()) },
            Images = { dto.Images.Select(i => i.ToGrpc()) }
        };

        return proto;
    }

    private static Proto.PriceInfoDto ToGrpc(this Application.Dtos.PriceInfoDto price)
    {
        var proto = new Proto.PriceInfoDto
        {
            AmountMinor = price.AmountMinor
        };

        if (price.OldAmountMinor is not null)
            proto.OldAmountMinor = price.OldAmountMinor.Value;

        return proto;
    }

    private static Proto.StockInfoDto ToGrpc(this Application.Dtos.StockInfoDto stock) =>
        new() { InStock = stock.InStock };

    private static Proto.ReviewSummaryDto ToGrpc(this Application.Dtos.ReviewSummaryDto summary) =>
        new()
        {
            AverageRating = (double)summary.AverageRating,
            TotalCount = summary.TotalCount,
        };

    private static Proto.ProductVariantAttributeDto ToGrpc(this Application.Dtos.ProductVariantAttributeDto attribute)
    {
        var proto = new Proto.ProductVariantAttributeDto
        {
            CharacteristicId = attribute.CharacteristicId.ToString(),
            Name = attribute.Name,
            Value = attribute.Value,
            CharType = attribute.CharType,
        };

        if (attribute.GroupName is not null)
            proto.GroupName = attribute.GroupName;

        return proto;
    }

    private static Proto.VariantImageDto ToGrpc(this Application.Dtos.VariantImageDto image) =>
        new()
        {
            Url = image.Url,
            Alt = image.Alt,
            SortOrder = image.SortOrder,
            IsMain = image.IsMain,
        };
}
