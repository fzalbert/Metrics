using System.Runtime.InteropServices;
using Collector.Service.Metrics;
using Collector.Service.MetricsSender;
using Collector.Service.Period;
using Collector.Service.SystemInformation;
using Collector.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Collector.Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services) =>
         services.AddHostedService<PeriodicTaskService>()
            .AddScoped<MetricsTimerService>()
            .AddScoped<IPeriodService, PeriodService>()
            .AddScoped<IMetricsSenderService, MockMetricsSenderService>()
            .AddSingleton<SignalRConnection>()
            .AddSingleton<PeriodApiClient>()
            .AddScoped<IMetricsService>((serviceProvider) =>
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return new MockMetricsService();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return new WindowsMetricsService();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return new LinuxMetricsService();

                throw new Exception("Wrong os");
            });
}