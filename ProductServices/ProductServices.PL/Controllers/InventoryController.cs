using Microsoft.AspNetCore.Mvc;
using ProductServices.BLL.Services.Interfaces;
using ProductServices.DAL.DTO.RequestDto;

namespace ProductServices.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> Get(long productId)
    {
        var inv = await _inventoryService.GetByProductIdAsync(productId);
        if (inv == null) return NotFound();
        return Ok(inv);
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateStock(long productId, int newStock)
    {
        var result = await _inventoryService.UpdateStockAsync(productId, newStock);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> ReserveStock(
    long productId,
    int quantity)
    {
        var success = await _inventoryService
            .ReserveStockAsync(productId, quantity);

        if (!success)
        {
            return BadRequest(new
            {
                Message = "Insufficient stock."
            });
        }

        return Ok(new
        {
            Message = "Stock reserved successfully."
        });
    }
}
