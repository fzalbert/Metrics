using Microsoft.Extensions.Configuration;

namespace Collector.Service.Period;

public class PeriodApiClient
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public PeriodApiClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new HttpClient();
    }

    public async Task<TimeSpan> GetActual()
    {
        
        var result = await _client.GetAsync($"http://localhost:5019/api/Period");

        if (!result.IsSuccessStatusCode)
            throw new Exception("");
        
        using var streamReader = new StreamReader(result.Content.ReadAsStream());
        var seconds = int.Parse(streamReader.ReadLine()!);
        
        return TimeSpan.FromSeconds(seconds);
    }
}