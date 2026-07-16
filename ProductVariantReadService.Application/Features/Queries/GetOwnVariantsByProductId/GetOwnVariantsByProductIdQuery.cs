using MediatR;
using migApp.Shared.Results;
using ProductVariantReadService.Application.Dtos;

namespace ProductVariantReadService.Application.Features.Queries.GetOwnVariantsByProductId;

public sealed record GetOwnVariantsByProductIdQuery(
    Guid ProductId,
    Guid VendorId,
    string Currency) : IRequest<IResult<IEnumerable<ProductVariantOwnerDto>>>;
