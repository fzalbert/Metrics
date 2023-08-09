using Common.Models;

namespace Collector.Service.Metrics;

public interface IMetricsService
{
    public Task<MetricsData> Get();
}