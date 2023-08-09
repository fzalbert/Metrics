using System.Data.Common;
using Npgsql;

namespace MetricsApi.Storage;

public interface IConnectorCreator
{
    public NpgsqlConnection Create();
}