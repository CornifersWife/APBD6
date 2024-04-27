using APBD6.Models;
using APBD6.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> AddProduct_Warehouse(ProductWarehouse productWarehouse) {
        return Ok();
    }
   
}