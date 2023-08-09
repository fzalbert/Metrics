using System.Diagnostics;
using System.Management;
using Common.Models;

namespace Collector.Service.Metrics;

public class WindowsMetricsService : IMetricsService
{
    private PerformanceCounter cpuCounter = new PerformanceCounter(
        "Processor", 
        "% Processor Time",
        "_Total"
        );
    
    
    public Task<MetricsData> Get()
    {
        
        var c = new PerformanceCounter();
        var query = "SELECT Capacity FROM Win32_PhysicalMemory";
        var searcher = new ManagementObjectSearcher(query);
        
        foreach (var WniPART in searcher.Get())
        {
            var capacity = Convert.ToUInt64(WniPART.Properties["Capacity"].Value);
            var capacityKB = capacity / 1024;
            var capacityMB = capacityKB / 1024;
            var capacityGB = capacityMB / 1024;
            System.Console.WriteLine("Size in KB: {0}, Size in MB: {1}, Size in GB: {2}", capacityKB, capacityMB, capacityGB);
        }
        throw new NotImplementedException();
    }
}