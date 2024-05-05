using APBD6.Models;
using APBD6.Models.DTOs;
using APBD6.Repositories;

namespace APBD6.Services;

public interface IWarehouseService {
    Task<int> AddProductWarehouse(AddProductWarehouse addProductWarehouse);
}

public class WarehouseService : IWarehouseService {
    private readonly IConfiguration configuration;
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly IProductWarehouseRepository productWarehouseRepository;
    private readonly IWarehouseRepository warehouseRepository;


    public WarehouseService(IConfiguration configuration, IOrderRepository orderRepository,
        IProductRepository productRepository, IProductWarehouseRepository productWarehouseRepository,
        IWarehouseRepository warehouseRepository) {
        this.configuration = configuration;
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.productWarehouseRepository = productWarehouseRepository;
        this.warehouseRepository = warehouseRepository;
    }


    public async Task<int> AddProductWarehouse(AddProductWarehouse addProductWarehouse) {
        if (!(await warehouseRepository.Exists(addProductWarehouse.IdWarehouse) &&
              await productRepository.Exists(addProductWarehouse.IdProduct)))
            throw new EntityNotFoundException("Warehouse or Product does not exist.");

        var idOrder = await orderRepository.FindOrder(addProductWarehouse);
        if (idOrder == -1)
            throw new OrderNotFoundException("Order not found or conditions not met.");

        if (await productWarehouseRepository.ExistsOrder(idOrder))
            throw new ConflictException("Order already processed.");

        var price = await productRepository.GetPrice(addProductWarehouse.IdProduct);
        var newProductWarehouse = new ProductWarehouse {
            IdProduct = addProductWarehouse.IdProduct,
            IdWarehouse = addProductWarehouse.IdWarehouse,
            IdOrder = idOrder,
            Amount = addProductWarehouse.Amount,
            Price = price,
            CreatedAt = addProductWarehouse.CreatedAt
        };

        var updateResult = await orderRepository.UpdateDate(idOrder);
        if (updateResult == -1)
            throw new Exception("Failed to update order date.");

        var idProductWarehouse = await productWarehouseRepository.Insert(newProductWarehouse);
        if (idProductWarehouse == -1)
            throw new Exception("Failed to insert product into warehouse.");

        return idProductWarehouse;
    }
}