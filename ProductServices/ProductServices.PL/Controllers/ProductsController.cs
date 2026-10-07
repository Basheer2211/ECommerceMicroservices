using Microsoft.AspNetCore.Mvc;
using ProductServices.BLL.Services.Interfaces;
using ProductServices.DAL.DTO.RequestDto;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ProductServices.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductImportService _productImportService;

    public ProductsController(IProductService productService, IProductImportService productImportService)
    {
        _productService = productService;
        _productImportService = productImportService;
    }
    [HttpGet("by-code/{productCode}")]
    public async Task<IActionResult> GetByProductCode(long productCode)
    {
        var product = await _productService.GetByProductCodeAsync(productCode);

        if (product == null)
            return NotFound();

        return Ok(product);
    }
    [HttpGet("by-category")]
    public async Task<IActionResult> GetByCategoryName(string categoryName)
    {
        try
        {
            var products = await _productService
                .GetByCategoryNameAsync(categoryName);

            return Ok(products);
        }
        catch (Exception ex)
        {
            return NotFound(new
            {
                Message = ex.Message
            });
        }
    }
    [HttpGet("by-price")]
    public async Task<IActionResult> GetByPriceRange(
    decimal price1,
    decimal price2)
    {
        var products = await _productService
            .GetByPriceRangeAsync(price1, price2);

        return Ok(products);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _productService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var p = await _productService.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductRequestDto dto)
    {
        try
        {
            var created = await _productService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(Get),
                new { id = created.ProductId },
                created);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] ProductRequestDto dto)
    {
        try
        {
            var ok = await _productService.UpdateAsync(id, dto);

            if (!ok)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var ok = await _productService.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { Message = "CSV file is required." });

        var result = await _productImportService.ImportAsync(file);

        if (!result.Success)
            return BadRequest(new { Message = result.Error });

        return Ok(new { Imported = result.Count });
    }
}