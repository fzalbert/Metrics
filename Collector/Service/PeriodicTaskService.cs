using Collector.Service.SystemInformation;
using Microsoft.Extensions.Hosting;

namespace Collector.Service;

public class PeriodicTaskService : BackgroundService
{
    private readonly MetricsTimerService _metricsTimerService;
    private readonly SignalRConnection _signalRConnection;

    public PeriodicTaskService(
        MetricsTimerService metricsTimerService,
        SignalRConnection signalRConnection
        )
    {
        _metricsTimerService = metricsTimerService;
        _signalRConnection = signalRConnection;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _signalRConnection.Connect()
            .ContinueWith(_ =>
            {
                _metricsTimerService.Start(stoppingToken);
            }, stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _metricsTimerService.Dispose();
        base.StopAsync(cancellationToken);
        return Task.CompletedTask;
    }
}