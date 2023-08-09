using MetricsApi.Hubs;
using MetricsApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MetricsApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeriodController
{
    private readonly PeriodService _periodService;
    private readonly IHubContext<MetricsHub> _hubContext;

    public PeriodController(PeriodService periodService, IHubContext<MetricsHub> hubContext)
    {
        _periodService = periodService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public int Get()
    {
        return _periodService.Get().Seconds;
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromQuery(Name = "Value")] int value)
    {
        _periodService.Set(TimeSpan.FromSeconds(value));
        await _hubContext.Clients.All.SendAsync(MetricsHub.GetPeriodMethod, _periodService.Get().Seconds);
        return new StatusCodeResult(204);
    }
}