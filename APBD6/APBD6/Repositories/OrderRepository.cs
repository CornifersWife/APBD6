using APBD6.Models;
using APBD6.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace APBD6.Repositories;

public interface IOrderRepository {
    Task<int> FindOrder(AddProductWarehouse productWarehouse);
    Task<int> UpdateDate(int idOrder);
}

public class OrderRepository : IOrderRepository {
    private readonly IConfiguration configuration;

    public OrderRepository(IConfiguration configuration) {
        this.configuration = configuration;
    }

    public async Task<int> FindOrder(AddProductWarehouse productWarehouse) {
        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"))) {
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"SELECT IdOrder FROM Order " +
                                     $"Where IdProduct = @1 " +
                                     $"AND Amount = @2 " +
                                     $"AND FulfilledAt IS NULL " +
                                     $"AND CreatedAt<@3";
            sqlCommand.Parameters.AddWithValue("@1", productWarehouse.IdProduct);
            sqlCommand.Parameters.AddWithValue("@2", productWarehouse.Amount);
            sqlCommand.Parameters.AddWithValue("@3", productWarehouse.CreatedAt);

            await sqlConnection.OpenAsync();
            var result = await sqlCommand.ExecuteScalarAsync();
            if (result is not null) {
                return (int)result;
            }

            return -1;
        }
    }

    public async Task<int> UpdateDate(int idOrder) {
        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"))) {
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"UPDATE Order " +
                                     $"SET FulfilledAt = @1 " +
                                     $"WHERE IdOrder = @2";
            sqlCommand.Parameters.AddWithValue("@1", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@2", idOrder);

            await sqlConnection.OpenAsync();
            var result = await sqlCommand.ExecuteScalarAsync();
            if (result is not null) {
                return (int)result;
            }

            return -1;
            //TODO chechk if its how we're supposed to do this
        }
    }
}