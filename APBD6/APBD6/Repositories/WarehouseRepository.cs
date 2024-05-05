using Microsoft.Data.SqlClient;

namespace APBD6.Repositories;

public interface IWarehouseRepository {
    Task<bool> Exists(int idWarehouse);
}

public class WarehouseRepository : IWarehouseRepository {
    private readonly IConfiguration configuration;

    public WarehouseRepository(IConfiguration configuration) {
        this.configuration = configuration;
    }

    public async Task<bool> Exists(int idWarehouse) {
        await using var sqlConnection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        await sqlConnection.OpenAsync();
        await using var sqlCommand = new SqlCommand();
        sqlCommand.Connection = sqlConnection;

        sqlCommand.CommandText = $"SELECT * FROM Warehouse Where IdWarehouse = @1";
        sqlCommand.Parameters.AddWithValue("@1", idWarehouse);
        var tmp = await sqlCommand.ExecuteScalarAsync();
        
        return tmp is not null;
    }
}