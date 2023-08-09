using System.Runtime.InteropServices;

namespace MetricsApi.Service;

public class PeriodService
{
    private TimeSpan _period;
    
    public PeriodService(IConfiguration configuration)
    {
        int period;
        var result = int.TryParse(configuration["Period"] ?? "0", out period);
        period = result ? period : 5;
        _period = TimeSpan.FromSeconds(period);
    }

    public TimeSpan Get()
    {
        return _period;
    }

    public void Set(TimeSpan period)
    {
        _period = period;
    }
}