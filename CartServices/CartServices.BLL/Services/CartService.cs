using CartServices.BLL.DTOs;
using CartServices.BLL.Interfaces;
using CartServices.DAL.Entities;
using CartServices.DAL.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace CartServices.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;
        private readonly IProductClient _productClient;

        public CartService(ICartRepository repo, IProductClient productClient)
        {
            _repo = repo;
            _productClient = productClient;
        }

        public async Task<CartResponse?> GetCartAsync(long customerId)
        {
            var cart = await _repo.GetCartByCustomerIdAsync(customerId);
            if (cart == null) return null;

            return MapToResponse(cart);
        }

        public async Task<CartResponse?> AddItemAsync(long customerId, AddCartItemRequest request)
        {
            if (request.Quantity <= 0) return null;

            var product = await _productClient.GetProductAsync(request.ProductId);
            if (product == null) return null;

            if (request.Quantity > product.Stock) return null;

            var cart = await _repo.GetCartByCustomerIdAsync(customerId);
            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = customerId,
                    CustomerName = $"Customer {customerId}",
                    CustomerAddress = string.Empty,
                    CreationDate = System.DateTime.UtcNow,
                    LastUpdateDate = System.DateTime.UtcNow
                };
                await _repo.AddCartAsync(cart);
            }

            var existing = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            if (existing != null)
            {
                existing.Quantity += request.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name ?? string.Empty,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price
                });
            }

            cart.LastUpdateDate = System.DateTime.UtcNow;
            await _repo.SaveChangesAsync();

            return MapToResponse(cart);
        }

        public async Task<CartResponse?> UpdateItemAsync(long customerId, long productId, UpdateCartItemRequest request)
        {
            if (request.Quantity <= 0) return null;

            var cart = await _repo.GetCartByCustomerIdAsync(customerId);
            if (cart == null) return null;

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (item == null) return null;

            var product = await _productClient.GetProductAsync(productId);
            if (product == null) return null;

            if (request.Quantity > product.Stock) return null;

            item.Quantity = request.Quantity;
            cart.LastUpdateDate = System.DateTime.UtcNow;
            await _repo.UpdateCartAsync(cart);
            await _repo.SaveChangesAsync();

            return MapToResponse(cart);
        }

        public async Task<bool> DeleteItemAsync(long customerId, long productId)
        {
            var cart = await _repo.GetCartByCustomerIdAsync(customerId);
            if (cart == null) return false;

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (item == null) return false;

            await _repo.DeleteCartItemAsync(item);
            cart.LastUpdateDate = System.DateTime.UtcNow;
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ClearCartAsync(long customerId)
        {
            var cart = await _repo.GetCartByCustomerIdAsync(customerId);
            if (cart == null) return false;

            await _repo.ClearCartItemsAsync(cart);
            cart.LastUpdateDate = System.DateTime.UtcNow;
            await _repo.SaveChangesAsync();

            return true;
        }

        private CartResponse MapToResponse(Cart cart)
        {
            var resp = new CartResponse
            {
                CartId = cart.CartId,
                CustomerId = cart.CustomerId,
                CustomerName = cart.CustomerName,
                CustomerAddress = cart.CustomerAddress,
                CreationDate = cart.CreationDate,
                LastUpdateDate = cart.LastUpdateDate,
            };

            foreach (var it in cart.CartItems)
            {
                var itemResp = new CartItemResponse
                {
                    ProductId = it.ProductId,
                    ProductName = it.ProductName,
                    Quantity = it.Quantity,
                    UnitPrice = it.UnitPrice,
                    SubTotal = it.Quantity * it.UnitPrice
                };
                resp.Items.Add(itemResp);
            }

            resp.TotalAmount = resp.Items.Sum(i => i.SubTotal);

            return resp;
        }
    }
}
