using Npgsql;

namespace MetricsApi.Storage;

public class PostgresConnectorCreator : IConnectorCreator, IDisposable
{
    private readonly IConfiguration _configuration;
    
    public PostgresConnectorCreator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public NpgsqlConnection Create()
    {
        var connectionString = _configuration["ConnectionString"];
        var dbConnection = new NpgsqlConnection(connectionString);
        
        // var selectCommand = new NpgsqlCommand("SELECT * FROM MyTable", _connection);
        // using var results = selectCommand.ExecuteReader();
        // results.Read();
        
        return dbConnection;
    }
    
    public void Dispose()
    {
    }
}