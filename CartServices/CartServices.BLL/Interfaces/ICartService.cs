using CartServices.BLL.DTOs;
using System.Threading.Tasks;

namespace CartServices.BLL.Interfaces
{
    public interface ICartService
    {
        Task<CartResponse?> GetCartAsync(long customerId);

        Task<CartResponse?> AddItemAsync(long customerId, AddCartItemRequest request);

        Task<CartResponse?> UpdateItemAsync(long customerId, long productId, UpdateCartItemRequest request);

        Task<bool> DeleteItemAsync(long customerId, long productId);

        Task<bool> ClearCartAsync(long customerId);
    }
}
