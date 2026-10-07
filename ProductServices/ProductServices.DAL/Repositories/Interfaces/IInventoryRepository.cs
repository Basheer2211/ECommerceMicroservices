using ProductServices.DAL.Models;

namespace ProductServices.DAL.Repositories.Interfaces;

public interface IInventoryRepository : IGenericRepository<Inventory>
{
    Task<Inventory?> GetByProductIdAsync(long productId);
    Task<bool> ReserveStockAsync(long productId, int quantity);
}
