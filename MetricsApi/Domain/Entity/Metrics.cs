using System.Data;
using System.Net;
using Common.Models;

namespace MetricsApi.Entity;


public class Metrics
{

    public Metrics()
    {
        
    }

    public Metrics(DataRow row)
    {
        Id = (long)row["id"];
        IpAddress = (string)row["ip_address"];
        RamFree = (long)row["ram_free"];
        RamAll = (long)row["ram_all"];
        PsdFree = (long)row["psd_free"];
        PsdAll = (long)row["psd_all"];
        CpuUsage = (double)row["cpu_usage"];
    }

    public Metrics(IPAddress ipAddress, MetricsData dto)
    {
        IpAddress = ipAddress.ToString();
        RamFree = dto.RAM.Free;
        RamAll = dto.RAM.All;
        PsdAll = dto.PSD.All;
        PsdFree = dto.PSD.Free;
        CpuUsage = dto.CpyPercent;
    }
    
    public long Id { get; set; }
    
    public string IpAddress { get; set; }
    
    public long RamFree { get; set; }
    public long RamAll { get; set; }
    
    public long PsdFree { get; set; }
    public long PsdAll { get; set; }
    
    public double CpuUsage { get; set; }
    
    //Добавить дату создания

}