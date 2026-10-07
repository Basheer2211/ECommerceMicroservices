using ProductServices.DAL.DTO.RequestDto;
using ProductServices.DAL.DTO.ResponseDto;

namespace ProductServices.BLL.Services.Interfaces;

public interface IInventoryService
{
    Task<InventoryResponseDto?> GetByProductIdAsync(long productId);
    Task<bool> UpdateStockAsync(long productId, int newStock);
    Task<bool> ReserveStockAsync(long productId, int quantity);
}
