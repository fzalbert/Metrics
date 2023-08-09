using System.Net;
using Common.Models;

namespace MetricsApi.Service.Metrics;

public interface IMetricsService
{
    Task Save(IPAddress fromIp, MetricsData data);
}