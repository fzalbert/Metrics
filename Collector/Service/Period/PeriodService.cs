using Microsoft.Extensions.Configuration;

namespace Collector.Service.Period;

public class PeriodService : IPeriodService
{

    private readonly IConfiguration _configuration;
    
    private TimeSpan _period;
    
    public void Set(TimeSpan value)
    {
        _period = value;
    }

    /// <summary>
    /// get actual period
    /// </summary>
    /// <returns>actual period</returns>
    TimeSpan IPeriodService.Get()
    {
        return _period;
    }
}