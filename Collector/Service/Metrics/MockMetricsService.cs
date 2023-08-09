using Common.Models;

namespace Collector.Service.Metrics;


public class MockMetricsService : IMetricsService
{
    public Task<MetricsData> Get()
    {
        var random = new Random();
        return Task.FromResult(new MetricsData(
            new MemoryData(random.Next(), random.Next()),
            new MemoryData(random.Next(), random.Next()),
            random.Next()
        ));
    }
}