namespace APBD6.Repositories;

public interface IRepository {
    Task<bool> Exists(int id);
}