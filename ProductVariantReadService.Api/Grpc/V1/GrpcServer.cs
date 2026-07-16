using Grpc.Core;
using MediatR;
using migApp.Shared.Grpc;
using ProductVariantReadService.Api.Grpc.Mapping;
using ProductVariantReadService.Api.Grpc.V1.Protos;
using ProductVariantReadService.Application.Features.Queries.GetOwnVariantsByProductId;
using ProductVariantReadService.Application.Features.Queries.GetVariantsByProductId;

namespace ProductVariantReadService.Api.Grpc.V1;

internal sealed class GrpcServer(IMediator mediator) : Protos.ProductVariantReadService.ProductVariantReadServiceBase
{
    public override async Task<GetVariantsByProductIdResponse> GetVariantsByProductId(GetVariantsByProductIdRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(new GetVariantsByProductIdQuery(
            Guid.Parse(request.ProductId),
            request.CurrencyCode), context.CancellationToken);
        return new GetVariantsByProductIdResponse
        {
            Variants = { result.ThrowIfFailure().Select(v => v.ToGrpc()) }
        };
    }

    public override async Task<GetOwnVariantsByProductIdResponse> GetOwnVariantsByProductId(GetOwnVariantsByProductIdRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(new GetOwnVariantsByProductIdQuery(
            Guid.Parse(request.ProductId),
            Guid.Parse(request.VendorId),
            request.CurrencyCode), context.CancellationToken);
        return new GetOwnVariantsByProductIdResponse
        {
            Variants = { result.ThrowIfFailure().Select(v => v.ToGrpc()) }
        };
    }
}