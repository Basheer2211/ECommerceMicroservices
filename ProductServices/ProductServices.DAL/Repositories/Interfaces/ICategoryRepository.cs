using ProductServices.DAL.Models;

namespace ProductServices.DAL.Repositories.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetWithProductsAsync(long id);
    Task SoftDelete(long id);   
    Task<IEnumerable<Category>> GetAllActiveCategoryAsync();
    Task<Category?> GetByNameAsync(string name);
    Task<Category?> GetByIdAsync(long id);
}
