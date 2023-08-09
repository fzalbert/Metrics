namespace Collector.Service.Period;

public interface IPeriodService
{
    TimeSpan Get();

    void Set(TimeSpan value);
}