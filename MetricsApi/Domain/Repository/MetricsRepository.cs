using System.Data;
using System.Data.SqlClient;
using MetricsApi.Entity;
using MetricsApi.Repository;
using MetricsApi.Storage;
using Npgsql;

namespace MetricsApi.Domain.Repository;

public class MetricsRepository : IMetricsRepository
{
    private const string INSERT_QUERY = @"
            INSERT INTO public.metrics
        (ip_address, ram_free, ram_all, psd_free, psd_all, cpu_usage)
        VALUES(@ip_address, @ram_free, @ram_all, @psd_free, @psd_all, @cpu_usage);
";

    private const string SELECT_ALL_QUERY = $@"
        SELECT * FROM public.metrics;
";
        
    private readonly IConnectorCreator _connectorCreator;
    public MetricsRepository(IConnectorCreator connectorCreator)
    {
        _connectorCreator = connectorCreator;
    }

    public async Task<long> Save(Metrics entity)
    {
        await using var connect = _connectorCreator.Create();
        await using var cmd = new NpgsqlCommand(INSERT_QUERY, connect);
        await connect.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();

        cmd.Parameters.AddWithValue("@ip_address", entity.IpAddress);
        cmd.Parameters.AddWithValue("@ram_free", entity.RamFree);
        cmd.Parameters.AddWithValue("@ram_all", entity.RamAll);
        cmd.Parameters.AddWithValue("@psd_free", entity.PsdFree);
        cmd.Parameters.AddWithValue("@psd_all", entity.PsdAll);
        cmd.Parameters.AddWithValue("@cpu_usage", entity.CpuUsage);
        
        if (result == null)
            throw new Exception();
        
        return (long) result;
    }

    public async Task<IEnumerable<Metrics>> GetAll()
    {
        await using var connect = _connectorCreator.Create();
        await using var cmd = new NpgsqlCommand(SELECT_ALL_QUERY, connect);
        await connect.OpenAsync();

        var dataTable =await (await cmd.ExecuteReaderAsync()).GetSchemaTableAsync();
        if (dataTable == null)
            return new List<Metrics>(0);


        var result = new List<Metrics>(dataTable.Rows.Count);
        foreach (DataRow row in dataTable.Rows)
        {
            result.Add(new Metrics(row));
        }
        return result;
    }
}