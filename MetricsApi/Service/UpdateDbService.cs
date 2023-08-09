using MetricsApi.Storage;
using Npgsql;

namespace MetricsApi.Domain.Storage;

public class UpdateDbService : IHostedService
{
    private const string CREATE_METRICS_TABLE_SQL = @"
        CREATE TABLE if not exists metrics (
	        id serial PRIMARY key,
	        ip_address cidr,
	        ram_free bigint,
	        ram_all bigint,
	        psd_free bigint,
	        psd_all bigint,
	        cpu_usage numeric
        );
";

    private readonly IConnectorCreator _connectorCreator;

    public UpdateDbService(IConnectorCreator connectorCreator)
    {
        _connectorCreator = connectorCreator;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var connect = _connectorCreator.Create();
        using var createCommand = new NpgsqlCommand(CREATE_METRICS_TABLE_SQL, connect);
        connect.Open();
        createCommand.ExecuteNonQuery();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}