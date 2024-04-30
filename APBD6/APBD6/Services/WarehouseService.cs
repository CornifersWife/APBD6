using APBD6.Models;
using APBD6.Repositories;

namespace APBD6.Services;

public interface IWarehouseService {
    Task<int> AddProductWarehouse(AddProductWarehouse addProductWarehouse);
}

public class WarehouseService : IWarehouseService {
    private readonly IConfiguration configuration;
    private readonly OrderRepository orderRepository;
    private readonly ProductRepository productRepository;
    private readonly ProductWarehouseRepository productWarehouseRepository;
    private readonly WarehouseRepository warehouseRepository;


    public WarehouseService(IConfiguration configuration, OrderRepository orderRepository,
        ProductRepository productRepository, ProductWarehouseRepository productWarehouseRepository,
        WarehouseRepository warehouseRepository) {
        this.configuration = configuration;
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.productWarehouseRepository = productWarehouseRepository;
        this.warehouseRepository = warehouseRepository;
    }


    public async Task<int> AddProductWarehouse(AddProductWarehouse addProductWarehouse) {
        if (!(
                await warehouseRepository.Exists(addProductWarehouse.IdWarehouse)
                && await productRepository.Exists(addProductWarehouse.IdProduct)
            ))
            throw new NotImplementedException();
        var idOrder = await orderRepository.FindOrder(addProductWarehouse);
        if (idOrder == -1)
            throw new NotImplementedException();
        if (await productWarehouseRepository.ExistsOrder(idOrder))
            throw new NotImplementedException();
        var price = await productRepository.GetPrice(addProductWarehouse.IdProduct);
        var newProductWarehouse = new ProductWarehouse {
            IdProduct = addProductWarehouse.IdProduct,
            IdWarehouse = addProductWarehouse.IdWarehouse,
            IdOrder = idOrder,
            Amount = addProductWarehouse.Amount,
            Price = price,
            CreatedAt = addProductWarehouse.CreatedAt
        };
        try {
            var xx = await orderRepository.UpdateDate(idOrder);
            if (xx == -1)
                throw new NotImplementedException();
            var idProductWarehouse = await productWarehouseRepository.Insert(newProductWarehouse);
            if (idProductWarehouse == -1)
                throw new NotImplementedException();
        }
        catch (Exception e) {
            //TODO
            throw new Exception(e.Message);
        }
        
    }
}