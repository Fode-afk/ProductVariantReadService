namespace ProductVariantReadService.Application.Caching;

internal static class CacheKeys
{
    internal static string VariantsByProductId(Guid productId, string currencyCode) =>
        $"variants:productId:{productId}:currency:{currencyCode}";
}
