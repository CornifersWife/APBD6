using APBD6.Models;
using Microsoft.Data.SqlClient;

namespace APBD6.Repositories;

public interface IProductWarehouseRepository {
    Task<bool> ExistsOrder(int idOrder);
    Task<int> Insert(ProductWarehouse productWarehouse);
}

public class ProductWarehouseRepository : IProductWarehouseRepository {
    private readonly IConfiguration configuration;

    public ProductWarehouseRepository(IConfiguration configuration) {
        this.configuration = configuration;
    }

    public async Task<bool> ExistsOrder(int idOrder) {
        await using var sqlConnection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);

        await using var sqlCommand = new SqlCommand();
        sqlCommand.Connection = sqlConnection;

        sqlCommand.CommandText = $"SELECT IdProduct FROM Product_Warehouse " +
                                 $"Where IdOrder = @1";
        sqlCommand.Parameters.AddWithValue("@1", idOrder);
        await sqlConnection.OpenAsync();
        if (await sqlCommand.ExecuteScalarAsync() is not null) {
            return true;
        }

        return false;
    }


    public async Task<int> Insert(ProductWarehouse productWarehouse) {
        await using var sqlConnection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        await sqlConnection.OpenAsync();

        await using var sqlCommand = new SqlCommand();
        sqlCommand.Connection = sqlConnection;

        sqlCommand.CommandText =
            $"INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
            $"VALUES (@1, @2, @3, @4, @5, @6);";
        sqlCommand.Parameters.AddWithValue("@1", productWarehouse.IdWarehouse);
        sqlCommand.Parameters.AddWithValue("@2", productWarehouse.IdProduct);
        sqlCommand.Parameters.AddWithValue("@3", productWarehouse.IdOrder);
        sqlCommand.Parameters.AddWithValue("@4", productWarehouse.Amount);
        sqlCommand.Parameters.AddWithValue("@5", productWarehouse.Amount * productWarehouse.Price);
        sqlCommand.Parameters.AddWithValue("@6", DateTime.Now);

        var result = await sqlCommand.ExecuteScalarAsync();
        var idProductWarehouse = Convert.ToInt32(result);
        return idProductWarehouse;
    }
}