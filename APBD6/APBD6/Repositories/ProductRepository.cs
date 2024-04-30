using APBD6.Models;
using Microsoft.Data.SqlClient;

namespace APBD6.Repositories;

public interface IProductRepository {
    Task<decimal> GetPrice(int idProduct);
    Task<bool> Exists(int idProduct);
}

public class ProductRepository : IProductRepository {
    private readonly IConfiguration configuration;

    public ProductRepository(IConfiguration configuration) {
        this.configuration = configuration;
    }

    public async Task<bool> Exists(int idProduct) {
        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"))) {
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"SELECT IdProduct " +
                                     $"FROM Product " +
                                     $"Where IdProduct = @1";
            sqlCommand.Parameters.AddWithValue("@1", idProduct);
            await sqlConnection.OpenAsync();
            if (await sqlCommand.ExecuteScalarAsync() is not null) {
                return true;
            }

            return false;
        }
    }

    public async Task<decimal> GetPrice(int idProduct) {
        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"))) {
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"SELECT Price " +
                                     $"FROM Product " +
                                     $"Where IdProduct = @1";
            sqlCommand.Parameters.AddWithValue("@1", idProduct);

            await sqlConnection.OpenAsync();
            var result = await sqlCommand.ExecuteScalarAsync();
            if (result is not null) {
                return (decimal)result;
            }
            return -1;
        }
    }
}