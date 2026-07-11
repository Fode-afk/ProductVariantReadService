namespace ProductVariantReadService.Application.Interfaces.Metrics;

public interface IProductVariantReadMetrics
{
    void RecordSnapshotNotFound(string snapshotType);
    void RecordSnapshotOutdated(string snapshotType);
    void RecordCacheHitOrMiss(string cacheKeyPrefix, bool wasHit);
    void RecordCacheInvalidation(string tag);
    void RecordHandlerError(string handlerName, string handlerType);
    void RecordHandlerDuration(double ms, string handlerName, string handlerType);
    void RecordProductVariantNotFound();
    void RecordProductVariantOutdated(string handlerName);
    void RecordProductVariantHiddenByVisibilityPolicy(string reason);
    void SetIndexedProductVariantsCount(int count);
}