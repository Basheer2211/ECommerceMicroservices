using CartServices.DAL.Entities;
using System.Threading.Tasks;

namespace CartServices.DAL.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByCustomerIdAsync(long customerId);

        Task AddCartAsync(Cart cart);

        Task UpdateCartAsync(Cart cart);

        Task DeleteCartItemAsync(CartItem cartItem);

        Task ClearCartItemsAsync(Cart cart);

        Task<bool> SaveChangesAsync();
    }
}
