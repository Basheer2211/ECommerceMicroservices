using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using ProductServices.DAL.Context;
using ProductServices.DAL.Models;
using ProductServices.DAL.Repositories.Interfaces;

namespace ProductServices.DAL.Repositories.Classes;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly ProductServicesDbContext _context;
    public ProductRepository(ProductServicesDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllActiveProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Inventory)
            .Include(p => p.AvailabilityStatus)
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }
    public async Task BulkInsertAsync(List<Product> products)
    {
        await _context.BulkInsertAsync(
            products,
            new BulkConfig
            {
                IncludeGraph = true
            });
    }

    public async Task<Product?> GetByProductCodeAsync(long productCode)
    {
        return await _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Inventory)
            .Include(p => p.AvailabilityStatus)
            .FirstOrDefaultAsync(p =>
                p.ProductCode == productCode &&
                !p.IsDeleted);
    }
    public async Task<List<Product>> GetByPriceRangeAsync(
    decimal minPrice,
    decimal maxPrice)
    {
        return await _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Inventory)
            .Include(p => p.AvailabilityStatus)
            .Where(p =>
                !p.IsDeleted &&
                p.Price >= minPrice &&
                p.Price <= maxPrice)
            .ToListAsync();
    }
    public async Task<List<Product>> GetByCategoryNameAsync(string categoryName)
    {
        return await _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Inventory)
            .Include(p => p.AvailabilityStatus)
            .Where(p =>
                !p.IsDeleted &&
                p.Categories.Any(c =>
                    c.Name == categoryName &&
                    !c.IsDeleted))
            .ToListAsync();
    }




    public async Task<Product?> GetProductByIdAsync(long id)
    {
        return await _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Inventory)
            .Include(p => p.AvailabilityStatus)
            .FirstOrDefaultAsync(p => p.ProductId == id&& !p.IsDeleted);
    }

    public async Task SoftDelete(long id)
    {
        await _context.Products.Where(p => p.ProductId == id).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsDeleted, true));
    }
}
