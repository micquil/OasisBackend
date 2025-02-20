using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient; 

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
