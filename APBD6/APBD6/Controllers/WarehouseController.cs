using System.Data;
using APBD6.Models;
using APBD6.Models.DTOs;
using APBD6.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace APBD6.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase {
    private readonly IConfiguration configuration;
    private IWarehouseService warehouseService;

    public WarehouseController(IConfiguration configuration, IWarehouseService warehouseService) {
        this.configuration = configuration;
        this.warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct_Warehouse(AddProductWarehouse productWarehouse) {
        try {
            int result = await warehouseService.AddProductWarehouse(productWarehouse);
            return CreatedAtAction(nameof(AddProduct_Warehouse), new { idOrder = result }, productWarehouse);
        }
        catch (EntityNotFoundException ex) {
            return NotFound(ex.Message);
        }
        catch (OrderNotFoundException ex) {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex) {
            return Conflict(ex.Message);
        }
    }

    [Route("2")]
    [HttpPost]
    public async Task<IActionResult> AddProduct_Warehouse2(AddProductWarehouse productWarehouse) {
        int newId;
        await using var sqlConnection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);

        var sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.CommandText = "AddProductToWarehouse";

        sqlCommand.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        sqlCommand.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        sqlCommand.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
        sqlCommand.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

        await sqlConnection.OpenAsync();

        try {
            newId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
            return StatusCode(201, newId);

        }
        catch (Exception ex) {
            var tmp = ex.Message;
            if (tmp.Contains("does not exist") || tmp.Contains("no order"))
                return NotFound(ex.Message);
        }

        return Ok();
    }
}