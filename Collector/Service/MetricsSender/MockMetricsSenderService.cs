using Collector.Services;
using Common.Models;
using Microsoft.Extensions.Logging;

namespace Collector.Service.MetricsSender;

public class MockMetricsSenderService : IMetricsSenderService
{
    private readonly ILogger<MockMetricsSenderService> _logger;

    public MockMetricsSenderService(ILogger<MockMetricsSenderService> logger)
    {
        _logger = logger;
    }

    public Task Connect()
    {
        return Task.FromResult(() => { });
    }

    public Task Send(MetricsData data)
    {
        return Task.FromResult(() => _logger.Log(LogLevel.Debug, data.ToString()));
    }
}