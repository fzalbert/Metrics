using Collector.Service.Period;
using Collector.Service.SystemInformation;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Collector.Service;

public class SignalRConnection : IDisposable
{
    private HubConnection? _connection;
    private readonly IConfiguration _configuration;
    private readonly IPeriodService _periodService;
    private readonly MetricsTimerService _metricsTimerService;
    private readonly PeriodApiClient _periodApiClient;

    public SignalRConnection(
        IConfiguration configuration,
        IPeriodService periodService,
        MetricsTimerService metricsTimerService,
        PeriodApiClient periodApiClient
    )
    {
        _configuration = configuration;
        _periodService = periodService;
        _metricsTimerService = metricsTimerService;
        _periodApiClient = periodApiClient;
    }

    public async Task Connect()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(GetMetricsUrl())
            .WithAutomaticReconnect()
            .Build();
        await _connection.StartAsync();
        
        var period = await _periodApiClient.GetActual();
        _periodService.Set(period);
        
        _connection.On<int>("GetPeriod", (time) =>
        {
            _periodService.Set(TimeSpan.FromSeconds(time));
            _metricsTimerService.Change();
        });
    }

    public HubConnection Get()
    {
        return _connection!;
    }

    private string GetMetricsUrl()
    {
        var serverSection = _configuration.GetSection("Server");
        var address = serverSection["Address"]!;
        var port = serverSection["Port"]!;
        return $"http://{address}:{port}/metrics-hub";
    }

    public void Dispose()
    {
    }
}