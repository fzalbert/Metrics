using Common.Models;

namespace Collector.Service.Metrics;

public class LinuxMetricsService : IMetricsService
{
    private const string  MEM_INFO_FILE_PATH = "/proc/meminfo";
    
    public Task<MetricsData> Get()
    {
        throw new NotImplementedException();
    }

    private MemoryData GetRam()
    {
        // all: MemTotal, free: MemFree + MemAvailable
        using var memFileStream =  File.Open(MEM_INFO_FILE_PATH, FileMode.Open, FileAccess.Read);
        using var streamReader = new StreamReader(memFileStream);

        
        var total = GetValueFromMemInfoLine(streamReader.ReadLine()!);
        var free = GetValueFromMemInfoLine(streamReader.ReadLine()!);
        var available = GetValueFromMemInfoLine(streamReader.ReadLine()!);
        
        return new MemoryData(total * 1024, (free + available) * 1024);
    }

    private MemoryData GetPsd()
    {
        return new MemoryData(0, 0);
    }

    private Task<double> GetCPUusage()
    {
        return Task.Run(() =>
        {
            var prevTime = CalculateCurrentCPUTime();
            Task.Delay(500);
            var currentTime = CalculateCurrentCPUTime();
            
            var percent = 100.0 * (1.0 - (currentTime.Item1 - prevTime.Item1) / (currentTime.Item2 - prevTime.Item2));
            
            return percent;
        });
    }
    
    private long GetValueFromMemInfoLine(string line)
    {
        if (long.TryParse(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1], out var result))
            return result;
        return -1;
    }

    /// <summary>
    /// Вычисление общего времени работы процессора
    /// </summary>
    /// <returns>Item1 - время простоя, Item2 - общее время</returns>
    private Tuple<float, float> CalculateCurrentCPUTime()
    {
        using var reader = new StreamReader("/proc/stat");
        
        var cpuLine = File
            .ReadAllLines("/proc/stat")
            .First()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(float.Parse)
            .ToArray();

        var idle = cpuLine[3];
        var total = cpuLine.Sum();
        
        return new Tuple<float, float>(idle, total);
    }
}