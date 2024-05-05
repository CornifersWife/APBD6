using APBD6.Models;
using APBD6.Models.DTOs;
using APBD6.Services;
using Microsoft.AspNetCore.Mvc;

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
}