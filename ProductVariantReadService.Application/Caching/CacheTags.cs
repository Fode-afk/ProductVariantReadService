namespace ProductVariantReadService.Application.Caching;

internal static class CacheTags
{
    internal static string VariantsByProductId(Guid productId) => $"variants:productId:{productId}";
}
