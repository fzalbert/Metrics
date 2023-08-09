using Collector.Service.Metrics;
using Collector.Service.Period;
using Collector.Services;

namespace Collector.Service.SystemInformation;

public class MetricsTimerService : IDisposable
{
    private readonly IMetricsSenderService _metricsSenderService;
    private readonly IMetricsService _metricsService;
    private readonly IPeriodService _periodService;

    private Timer? _timer;

    public MetricsTimerService(
        IMetricsSenderService metricsSenderService,
        IMetricsService metricsService,
        IPeriodService periodService
    )
    {
        _metricsSenderService = metricsSenderService;
        _metricsService = metricsService;
        _periodService = periodService;
    }

    private void Send()
    {
        var metrics = _metricsService.Get();
        metrics.Wait();
        _metricsSenderService.Send(metrics.Result);
    }

    public void Start(CancellationToken token)
    {
        _timer = new Timer((_) => Send(), null, TimeSpan.Zero,
            _periodService.Get());
    }

    public void Change()
    {
        _timer?.Change(TimeSpan.Zero, _periodService.Get());
    }

    public void Dispose()
    {
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
    }
}