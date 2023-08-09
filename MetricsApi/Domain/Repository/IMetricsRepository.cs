using MetricsApi.Entity;

namespace MetricsApi.Repository;

public interface IMetricsRepository
{
    Task<long> Save(Metrics entity);
    
    Task<IEnumerable<Metrics>> GetAll();
}