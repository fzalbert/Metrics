// See https://aka.ms/new-console-template for more information


using System.Diagnostics.Tracing;
using Collector.Service;
using Collector.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class Program
{
    private static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddBusinessLogic();
            })
            .ConfigureAppConfiguration((_, configuration) =>
            {
                configuration
                    .AddJsonFile("appsettings.json", optional: true);
            })
            .Build();
        host.Run();
    }

    class TestEventListener : EventListener, IHostedService
    {
        private readonly IMetricsSenderService _metricsSenderService;
        public TestEventListener(IMetricsSenderService metricsSenderService)
        {
            _metricsSenderService = metricsSenderService;
        }
        
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            var filterArgs = new Dictionary<string,string>();
            filterArgs["EventCounterIntervalSec"] = "1";
            
            if(eventSource.Name == "System.Runtime")
                EnableEvents(eventSource, EventLevel.Verbose, EventKeywords.All, filterArgs!);
            base.OnEventSourceCreated(eventSource);
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            base.OnEventWritten(eventData);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            base.Dispose();
            throw new NotImplementedException();
        }
    }
}