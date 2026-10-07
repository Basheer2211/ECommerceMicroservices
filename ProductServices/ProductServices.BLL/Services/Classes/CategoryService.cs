using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductServices.BLL.Services.Interfaces;
using ProductServices.DAL.DTO.RequestDto;
using ProductServices.DAL.DTO.ResponseDto;
using ProductServices.DAL.Models;
using ProductServices.DAL.Repositories.Interfaces;

namespace ProductServices.BLL.Services.Classes;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryService(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryRequestDto dto)
    {
        var existing = await _categoryRepo.GetByNameAsync(dto.Name);

        if (existing != null)
            throw new Exception("Category with this name already exists.");

        var cat = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _categoryRepo.AddAsync(cat);
        await _categoryRepo.SaveChangesAsync();

        return new CategoryResponseDto
        {
            CategoryId = cat.CategoryId,
            Name = cat.Name,
            Description = cat.Description
        };
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var cat = await _categoryRepo.GetByIdAsync(id);
        if (cat == null) return false;
        await _categoryRepo.SoftDelete(id);
        await _categoryRepo.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var cats = await _categoryRepo.GetAllActiveCategoryAsync();
        return cats.Select(c => new CategoryResponseDto
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description
        }).ToList();
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(long id)
    {
        var c = await _categoryRepo.GetWithProductsAsync(id);
        if (c == null) return null;
        return new CategoryResponseDto
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description
        };
    }

    public async Task<bool> UpdateAsync(long id, CategoryRequestDto dto)
    {
        var cat = await _categoryRepo.GetByIdAsync(id);
        if (cat == null) return false;
        cat.Name = dto.Name;
        cat.Description = dto.Description;
        _categoryRepo.Update(cat);
        await _categoryRepo.SaveChangesAsync();
        return true;
    }

    
}
