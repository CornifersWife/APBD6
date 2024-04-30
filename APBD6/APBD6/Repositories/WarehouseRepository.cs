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
        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"))) {
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"SELECT IdProduct FROM Warehouse Where IdWarehouse = @1";
            sqlCommand.Parameters.AddWithValue("@1", idWarehouse);
            await sqlConnection.OpenAsync();
            return await sqlCommand.ExecuteScalarAsync() is not null;
        }
    }
}