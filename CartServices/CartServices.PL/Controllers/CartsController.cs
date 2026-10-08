using CartServices.BLL.DTOs;
using CartServices.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CartServices.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCart(long customerId)
        {
            var cart = await _cartService.GetCartAsync(customerId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("{customerId}/items")]
        public async Task<IActionResult> AddItem(long customerId, [FromBody] AddCartItemRequest request)
        {
            if (request == null || request.Quantity <= 0) return BadRequest();

            var cart = await _cartService.AddItemAsync(customerId, request);
            if (cart == null) return BadRequest();

            return CreatedAtAction(nameof(GetCart), new { customerId = cart.CustomerId }, cart);
        }

        [HttpPut("{customerId}/items/{productId}")]
        public async Task<IActionResult> UpdateItem(long customerId, long productId, [FromBody] UpdateCartItemRequest request)
        {
            if (request == null || request.Quantity <= 0) return BadRequest();

            var cart = await _cartService.UpdateItemAsync(customerId, productId, request);
            if (cart == null) return NotFound();

            return Ok(cart);
        }

        [HttpDelete("{customerId}/items/{productId}")]
        public async Task<IActionResult> DeleteItem(long customerId, long productId)
        {
            var ok = await _cartService.DeleteItemAsync(customerId, productId);
            if (!ok) return NotFound();
            return Ok();
        }

        [HttpDelete("{customerId}/items")]
        public async Task<IActionResult> ClearCart(long customerId)
        {
            var ok = await _cartService.ClearCartAsync(customerId);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}
