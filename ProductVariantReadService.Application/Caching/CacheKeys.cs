namespace ProductVariantReadService.Application.Caching;

internal static class CacheKeys
{
    internal static string PublicVariantsByProductId(Guid productId, string currencyCode) =>
        $"public-variants:productId:{productId}:currency:{currencyCode}";

    internal static string OwnVariantsByProductId(Guid productId, string currencyCode) =>
        $"own-variants:productId:{productId}:currency:{currencyCode}";
}
