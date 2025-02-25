// using System.Data;
// using Npgsql;

// public class DapperContext
// {
//     private readonly string _connectionString;

//     public DapperContext(string connectionString)
//     {
//         _connectionString = connectionString;
//     }

//     public IDbConnection CreateConnection()
//         => new NpgsqlConnection(_connectionString);
// }


using System.Data;
using Microsoft.Data.SqlClient;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
