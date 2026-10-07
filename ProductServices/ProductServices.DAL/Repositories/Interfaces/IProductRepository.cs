using ProductServices.DAL.Models;

namespace ProductServices.DAL.Repositories.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetProductByIdAsync(long id);
    Task SoftDelete(long id);   
    Task<IEnumerable<Product>> GetAllActiveProductsAsync();
    Task<Product?> GetByProductCodeAsync(long productCode);
    Task<List<Product>> GetByCategoryNameAsync(string categoryName);
    Task<List<Product>> GetByPriceRangeAsync(
  decimal minPrice,
  decimal maxPrice);

    Task BulkInsertAsync(List<Product> products);
}
