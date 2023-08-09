using Common.Models;

namespace Collector.Services;

public interface IMetricsSenderService
{
    Task Send(MetricsData data);
}