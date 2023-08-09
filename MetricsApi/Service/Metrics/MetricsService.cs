using System.Net;
using Common.Models;
using MetricsApi.Repository;

namespace MetricsApi.Service.Metrics;

public class MetricsService : IMetricsService
{
    private readonly IMetricsRepository _repository;

    public MetricsService(IMetricsRepository repository)
    {
        _repository = repository;
    }

    public Task Save(IPAddress fromIp, MetricsData data)
    {
        var entity = new Entity.Metrics(fromIp, data);

        return _repository.Save(entity).ContinueWith((id) =>
        { });
    }
}