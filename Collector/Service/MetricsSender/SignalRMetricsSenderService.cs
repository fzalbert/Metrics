using Collector.Services;
using Common.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Collector.Service.MetricsSender;

public class SignalRMetricsSenderService : IMetricsSenderService
{
    private readonly ILogger<SignalRMetricsSenderService> _logger;
    private readonly SignalRConnection _connection;
    
    public SignalRMetricsSenderService(
        ILogger<SignalRMetricsSenderService> logger,
        SignalRConnection connection
        )
    {
        _logger = logger;
        _connection = connection;
    }
    public async Task Send(MetricsData data)
    {
        var hubConnect = _connection.Get();
        
        _logger.Log(LogLevel.Debug, data.ToString());
        await hubConnect.SendAsync("Post", data);
    }
}