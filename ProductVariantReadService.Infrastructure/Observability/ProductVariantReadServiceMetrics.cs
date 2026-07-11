using ProductVariantReadService.Application.Interfaces.Metrics;
using System.Diagnostics.Metrics;

namespace ProductVariantReadService.Infrastructure.Observability;

public sealed class ProductVariantReadServiceMetrics : IDisposable, IProductVariantReadMetrics
{
    public const string MeterName = "ProductVariantReadService";
    private readonly Meter _meter;

    private readonly Counter<long> _snapshotNotFound;
    private readonly Counter<long> _snapshotOutdated;

    private readonly Counter<long> _cacheHits;
    private readonly Counter<long> _cacheMisses;
    private readonly Counter<long> _cacheInvalidations;

    private readonly Counter<long> _handlerErrors;
    private readonly Histogram<double> _handlerDuration;

    private readonly Counter<long> _productVariantsNotFound;
    private readonly Counter<long> _productVariantsOutdated;
    private readonly Counter<long> _productVariantsHiddenByVisibilityPolicy;

    private int _indexedProductVariantsCount_value;
    private readonly ObservableGauge<int> _indexedProductVariantsCount;

    public ProductVariantReadServiceMetrics()
    {
        _meter = new Meter(MeterName);

        _snapshotNotFound = _meter.CreateCounter<long>(
            "product_variant_read.snapshots.not_found",
            description: "Snapshot missing when projection event arrived — possible race condition");

        _snapshotOutdated = _meter.CreateCounter<long>(
            "product_variant_read.snapshots.outdated",
            description: "Projection update skipped because incoming version is stale");

        _cacheHits = _meter.CreateCounter<long>(
            "product_variant_read.cache.hits");

        _cacheMisses = _meter.CreateCounter<long>(
            "product_variant_read.cache.misses");

        _cacheInvalidations = _meter.CreateCounter<long>(
            "product_variant_read.cache.invalidations",
            description: "Number of RemoveByTag operations triggered");

        _handlerErrors = _meter.CreateCounter<long>(
            "product_variant_read.handlers.errors");

        _handlerDuration = _meter.CreateHistogram<double>(
            "product_variant_read.handlers.duration",
            unit: "ms");

        _productVariantsNotFound = _meter.CreateCounter<long>(
            "product_variant_read.product_variants.not_found");

        _productVariantsOutdated = _meter.CreateCounter<long>(
            "product_variant_read.product_variants.outdated",
            description: "Projection update skipped because incoming version is stale");

        _productVariantsHiddenByVisibilityPolicy = _meter.CreateCounter<long>(
            "product_variant_read.product_variants.hidden_by_visibility_policy",
            description: "ProductVariant exists but was filtered out (draft, locked, suspended)");

        _indexedProductVariantsCount = _meter.CreateObservableGauge(
            "product_variant_read.product_variants.indexed",
            () => _indexedProductVariantsCount_value);
    }

    public void RecordSnapshotNotFound(string snapshotType) =>
        _snapshotNotFound.Add(1, new KeyValuePair<string, object?>("type", snapshotType));

    public void RecordSnapshotOutdated(string snapshotType) =>
        _snapshotOutdated.Add(1, new KeyValuePair<string, object?>("type", snapshotType));

    public void RecordCacheHitOrMiss(string cacheKeyPrefix, bool wasHit)
    {
        if (wasHit)
            _cacheHits.Add(1, new KeyValuePair<string, object?>("key_prefix", cacheKeyPrefix));
        else
            _cacheMisses.Add(1, new KeyValuePair<string, object?>("key_prefix", cacheKeyPrefix));
    }

    public void RecordCacheInvalidation(string tag) =>
        _cacheInvalidations.Add(1, new KeyValuePair<string, object?>("tag_type", tag));

    public void RecordHandlerError(string handlerName, string handlerType) =>
        _handlerErrors.Add(1,
            new KeyValuePair<string, object?>("handler", handlerName),
            new KeyValuePair<string, object?>("type", handlerType));

    public void RecordHandlerDuration(double ms, string handlerName, string handlerType) =>
        _handlerDuration.Record(ms,
            new KeyValuePair<string, object?>("handler", handlerName),
            new KeyValuePair<string, object?>("type", handlerType));

    public void RecordProductVariantNotFound() => _productVariantsNotFound.Add(1);

    public void RecordProductVariantOutdated(string handlerName) =>
        _productVariantsOutdated.Add(1, new KeyValuePair<string, object?>("handler", handlerName));

    public void RecordProductVariantHiddenByVisibilityPolicy(string reason) =>
        _productVariantsHiddenByVisibilityPolicy.Add(1, new KeyValuePair<string, object?>("reason", reason));

    public void SetIndexedProductVariantsCount(int count) =>
        _indexedProductVariantsCount_value = count;

    public void Dispose() => _meter.Dispose();
}