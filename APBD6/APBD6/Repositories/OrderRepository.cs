namespace APBD6.Repositories;

public interface IOrderRepository : IRepository {
}
public class OrderRepository : IOrderRepository {
    public Task<bool> Exists(int id) {
        throw new NotImplementedException();
    }

    public Task Create() {
        throw new NotImplementedException();
    }
}