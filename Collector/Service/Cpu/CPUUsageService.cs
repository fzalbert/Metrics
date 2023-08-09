using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Collector.Service.Cpu;

public class CPUUsageService : BackgroundService
{
    private float _prevIdleTime = 0;
    private float _prevTotalTime = 0;
    
    private Timer? _timer;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Process.GetCurrentProcess();
        var time = CalculateCurrentCPUTime();
        _prevIdleTime = time.Item1;
        _prevTotalTime = time.Item2;
        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        
        
        // _timer = new Timer(PeriodicCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }
    
    // private async void PeriodicCallback(object state)
    // {
    //     await _systemInformationService.Send();
    // }
    
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
    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        base.StopAsync(cancellationToken);
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}