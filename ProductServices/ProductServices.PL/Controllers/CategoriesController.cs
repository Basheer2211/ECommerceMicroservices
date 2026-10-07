using Microsoft.AspNetCore.Mvc;
using ProductServices.BLL.Services.Interfaces;
using ProductServices.DAL.DTO.RequestDto;

namespace ProductServices.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _categoryService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var c = await _categoryService.GetByIdAsync(id);
        if (c == null) return NotFound();
        return Ok(c);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryRequestDto dto)
    {
        try
        {
            var created = await _categoryService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(Get),
                new { id = created.CategoryId },
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
    public async Task<IActionResult> Update(long id, [FromBody] CategoryRequestDto dto)
    {
        var ok = await _categoryService.UpdateAsync(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }
    

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var ok = await _categoryService.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
