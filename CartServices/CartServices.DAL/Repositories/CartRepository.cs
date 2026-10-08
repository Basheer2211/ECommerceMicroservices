using CartServices.DAL.Data;
using CartServices.DAL.Entities;
using CartServices.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CartServices.DAL.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _db;

        public CartRepository(CartDbContext db)
        {
            _db = db;
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _db.Carts.AddAsync(cart);
        }

        public async Task<Cart?> GetCartByCustomerIdAsync(long customerId)
        {
            return await _db.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public Task UpdateCartAsync(Cart cart)
        {
            _db.Carts.Update(cart);
            return Task.CompletedTask;
        }

        public Task DeleteCartItemAsync(CartItem cartItem)
        {
            _db.CartItems.Remove(cartItem);
            return Task.CompletedTask;
        }

        public Task ClearCartItemsAsync(Cart cart)
        {
            _db.CartItems.RemoveRange(cart.CartItems);
            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _db.SaveChangesAsync()) > 0;
        }
    }
}
