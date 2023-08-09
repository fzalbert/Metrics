using Common.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using MetricsApi.Service;
using MetricsApi.Service.Metrics;

namespace MetricsApi.Hubs;

public class MetricsHub : Hub
{
    public const string GetPeriodMethod = "GetPeriod";
    
    private readonly IMetricsService _metricsService;
    
    public MetricsHub(IMetricsService metricsService)
    {
        _metricsService = metricsService;
    }
    
    public Task Post(MetricsData message)
    {
        var remoteIp = Context.Features.Get<IHttpConnectionFeature>()!.RemoteIpAddress!;
        return _metricsService.Save(remoteIp, message);
    }
}