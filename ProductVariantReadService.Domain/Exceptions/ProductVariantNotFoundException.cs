namespace ProductVariantReadService.Domain.Exceptions;

public sealed class ProductVariantNotFoundException(Guid id)
    : Exception($"Product variant with ID {id} not found. " +
                $"The projection event may have arrived before the product variant was created."), IExpectedException;