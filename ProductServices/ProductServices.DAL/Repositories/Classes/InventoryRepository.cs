using Microsoft.EntityFrameworkCore;
using ProductServices.DAL.Context;
using ProductServices.DAL.Models;
using ProductServices.DAL.Repositories.Interfaces;

namespace ProductServices.DAL.Repositories.Classes;

public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
{
    private readonly ProductServicesDbContext _context;
    public InventoryRepository(ProductServicesDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Inventory?> GetByProductIdAsync(long productId)
    {
        return await _context.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.ProductId == productId);
    }
    public async Task<bool> ReserveStockAsync(long productId, int quantity)
    {
        var rowsAffected = await _context.Database.ExecuteSqlInterpolatedAsync($@"
        UPDATE Inventory
        SET Stock = Stock - {quantity}
        WHERE ProductId = {productId}
          AND Stock >= {quantity}");

        return rowsAffected > 0;
    }
}
