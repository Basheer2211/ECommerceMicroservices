using System.Threading.Tasks;
using ProductServices.BLL.Services.Interfaces;
using ProductServices.DAL.DTO.ResponseDto;
using ProductServices.DAL.Models;
using ProductServices.DAL.Repositories.Interfaces;

namespace ProductServices.BLL.Services.Classes;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepo;

    public InventoryService(IInventoryRepository inventoryRepo)
    {
        _inventoryRepo = inventoryRepo;
    }

    public async Task<InventoryResponseDto?> GetByProductIdAsync(long productId)
    {
        var inv = await _inventoryRepo.GetByProductIdAsync(productId);
        if (inv == null) return null;
        return new InventoryResponseDto { ProductId = inv.ProductId, Stock = inv.Stock };
    }

    public async Task<bool> UpdateStockAsync(long productId, int newStock)
    {
        var inv = await _inventoryRepo.GetByProductIdAsync(productId);
        if (inv == null)
        {
            return false;
        }
        else
        {
            inv.Stock = newStock;
            _inventoryRepo.Update(inv);
        }

        await _inventoryRepo.SaveChangesAsync();
        return true;
    }
    public async Task<bool> ReserveStockAsync(long productId, int quantity)
    {
        if (quantity <= 0)
            return false;

        return await _inventoryRepo.ReserveStockAsync(productId, quantity);
    }
}
