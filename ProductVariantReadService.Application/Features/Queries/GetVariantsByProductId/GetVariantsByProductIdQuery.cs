using MediatR;
using migApp.Shared.Results;
using ProductVariantReadService.Application.Dtos;

namespace ProductVariantReadService.Application.Features.Queries.GetVariantsByProductId;

public sealed record GetVariantsByProductIdQuery(
    Guid ProductId,
    string Currency) : IRequest<IResult<IEnumerable<ProductVariantPublicDto>>>;