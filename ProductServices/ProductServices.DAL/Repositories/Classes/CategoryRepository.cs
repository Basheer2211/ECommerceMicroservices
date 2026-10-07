using Microsoft.EntityFrameworkCore;
using ProductServices.DAL.Context;
using ProductServices.DAL.Models;
using ProductServices.DAL.Repositories.Interfaces;

namespace ProductServices.DAL.Repositories.Classes;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly ProductServicesDbContext _context;
    public CategoryRepository(ProductServicesDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllActiveCategoryAsync()
    {
       return await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
    }
    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == name && !c.IsDeleted);
    }
    
    public async Task<Category?> GetByIdAsync(long id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id && !c.IsDeleted);
    }

    public async Task<Category?> GetWithProductsAsync(long id)
    {
        return await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.CategoryId == id && !c.IsDeleted);
    }

    public async Task SoftDelete(long id)
    {
        await _context.Categories.Where(c => c.CategoryId == id).ExecuteUpdateAsync(c => c.SetProperty(c => c.IsDeleted, true));
    }
}
