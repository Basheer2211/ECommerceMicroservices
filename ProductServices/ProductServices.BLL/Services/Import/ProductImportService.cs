using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using ProductServices.BLL.Services.Import;
using ProductServices.BLL.Services.Interfaces;
using ProductServices.DAL.DTO.RequestDto;
using ProductServices.DAL.Models;
using ProductServices.DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
namespace ProductServices.BLL.Services;

public class ProductImportService : IProductImportService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IProductCsvParser _csvParser;
    private readonly IProductImportValidator _validator;

    public ProductImportService(
        IProductRepository productRepo,
        ICategoryRepository categoryRepo,
        IProductCsvParser csvParser,
        IProductImportValidator validator)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _csvParser = csvParser;
        _validator = validator;
    }

    public async Task<(bool Success, string? Error, int Count)> ImportAsync(
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return (false, "CSV file is required.", 0);

        // 1. Parse CSV
        List<ProductImportDto> items;

        try
        {
            using var stream = file.OpenReadStream();

            items = await _csvParser.ParseAsync(stream);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, 0);
        }

        if (!items.Any())
            return (false, "No products to import.", 0);

        // 2. Validate imported data
        var validationError = _validator.Validate(items);

        if (validationError != null)
            return (false, validationError, 0);

        // 3. Get all category IDs from CSV
        var categoryIds = items
            .SelectMany(p => p.CategoryIds)
            .Distinct()
            .ToList();

        // 4. Load categories once
        var categories = await _categoryRepo.FindAsync(
            c => categoryIds.Contains(c.CategoryId)
                 && !c.IsDeleted);

        var existingCategoryIds = categories
            .Select(c => c.CategoryId)
            .ToHashSet();

        // 5. Check missing categories
        var missingCategoryIds = categoryIds
            .Where(id => !existingCategoryIds.Contains(id))
            .ToList();

        if (missingCategoryIds.Any())
        {
            return (
                false,
                $"The following category IDs do not exist: " +
                $"{string.Join(',', missingCategoryIds)}",
                0
            );
        }

        // 6. Create lookup dictionary
        var categoryMap = categories
            .ToDictionary(c => c.CategoryId);

        // 7. Build Product entities
        var products = new List<Product>();

        foreach (var item in items)
        {
            var product = new Product
            {
                ProductCode = item.ProductCode,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,

                Inventory = new Inventory
                {
                    Stock = item.Quantity
                },

                AvailabilityStatusId =
                    item.Quantity == 0 ? 2 : 1
            };

            foreach (var categoryId in item.CategoryIds)
            {
                product.Categories.Add(
                    categoryMap[categoryId]);
            }

            products.Add(product);
        }

        // 8. Bulk insert
        try
        {
            await _productRepo.BulkInsertAsync(products);

            return (true, null, products.Count);
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            return (
                false,
                "One or more ProductCodes already exist in the database.",
                0
            );
        }
        catch (Exception)
        {
            return (
                false,
                "An error occurred while importing the products.",
                0
            );
        }
    }
}