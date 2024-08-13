using System.Data;
using Dapper;
using Npgsql;

namespace Catalog.API.Data;

public class DataContextDapper
{
    private readonly IConfiguration _config;
    private readonly string _connectionName = "Defaultconnection";

    public DataContextDapper(IConfiguration config)
    {
        _config = config;
    }

    public IEnumerable<T> LoadData<T>(string sql)
    {
        using (IDbConnection dbConnection = new NpgsqlConnection(_config.GetConnectionString(_connectionName)))
        {
            dbConnection.Open();
            return dbConnection.Query<T>(sql);
        }
    }

    public bool ExecuteSql(string sql)
    {
        using (IDbConnection dbConnection = new NpgsqlConnection(_config.GetConnectionString(_connectionName)))
        {
            dbConnection.Open();
            return dbConnection.Execute(sql) > 0;
        }
    }

    public bool ExecuteSqlWithParameters(string sql, IDictionary<string, object> parameters)
    {
        using (IDbConnection dbConnection = new NpgsqlConnection(_config.GetConnectionString(_connectionName)))
        {
            dbConnection.Open();
            return dbConnection.Execute(sql, parameters) > 0;
        }
    }
}
