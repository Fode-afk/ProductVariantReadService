using migApp.Shared.Results;

namespace ProductVariantReadService.Domain.Errors;

public static class ProductVariantErrors
{
    public static Error NotFound() =>
        Error.NotFound(ProductVariantErrorCodes.NotFound,
            "Product variant not found.");
}

public static class ProductVariantErrorCodes
{
    public const string NotFound = "ProductVariant.NotFound";
}