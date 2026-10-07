using Microsoft.AspNetCore.Http;
using ProductServices.BLL.Services.Interfaces;
using ProductServices.DAL.DTO.RequestDto;
using ProductServices.DAL.DTO.ResponseDto;
using ProductServices.DAL.Models;
using ProductServices.DAL.Repositories.Interfaces;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;
using CsvHelper;
using System.Globalization;
using Microsoft.AspNetCore.Http;
namespace ProductServices.BLL.Services.Classes;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IInventoryRepository _inventoryRepo;

    public ProductService(
        IProductRepository productRepo,
        ICategoryRepository categoryRepo,
        IInventoryRepository inventoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _inventoryRepo = inventoryRepo;
    }

      
    public async Task<IEnumerable<ProductResponseDto>> GetByPriceRangeAsync(
    decimal price1,
    decimal price2)
    {
        var minPrice = Math.Min(price1, price2);
        var maxPrice = Math.Max(price1, price2);

        var products = await _productRepo
            .GetByPriceRangeAsync(minPrice, maxPrice);

        return products.Select(MapToResponse).ToList();
    }
    public async Task<IEnumerable<ProductResponseDto>> GetByCategoryNameAsync(
      string categoryName)
    {
        var category = await _categoryRepo.GetByNameAsync(categoryName);

        if (category == null)
            throw new Exception($"Category '{categoryName}' not found.");

        var products = await _productRepo.GetByCategoryNameAsync(categoryName);

        return products.Select(MapToResponse).ToList();
    }
    public async Task<ProductResponseDto> CreateAsync(ProductRequestDto dto)
    {
        if (dto.Quantity < 0)
            throw new Exception("Quantity cannot be negative.");

        var product = new Product
        {
            ProductCode = dto.ProductCode,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,

            Inventory = new Inventory
            {
                Stock = dto.Quantity
            },

            AvailabilityStatusId = dto.Quantity == 0 ? 2 : 1
        };

        // Attach categories
        foreach (var catId in dto.CategoryIds)
        {
            var cat = await _categoryRepo.GetByIdAsync(catId);

            if (cat == null)
                throw new Exception($"Category with ID {catId} not found.");

            product.Categories.Add(cat);
        }

        await _productRepo.AddAsync(product);
        await _productRepo.SaveChangesAsync();

        var created = await _productRepo.GetProductByIdAsync(product.ProductId);

        return MapToResponse(created!);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null)
            return false;

        await _productRepo.SoftDelete(id);
        await _productRepo.SaveChangesAsync();

        return true;
    }
    public async Task<ProductResponseDto?> GetByProductCodeAsync(long productCode)
    {
        var product = await _productRepo.GetByProductCodeAsync(productCode);

        if (product == null)
            return null;

        return MapToResponse(product);
    }
    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = await _productRepo.GetAllActiveProductsAsync();

        return products
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<ProductResponseDto?> GetByIdAsync(long id)
    {
        var product = await _productRepo.GetProductByIdAsync(id);

        if (product == null)
            return null;

        return MapToResponse(product);
    }

    public async Task<bool> UpdateAsync(long id, ProductRequestDto dto)
    {
        if (dto.Quantity < 0)
            throw new Exception("Quantity cannot be negative.");

        var product = await _productRepo.GetProductByIdAsync(id);

        if (product == null)
            return false;

        // Update inventory
        product.Inventory.Stock = dto.Quantity;

        // Update availability status
        product.AvailabilityStatusId = dto.Quantity == 0 ? 2 : 1;

        // Update product information
        product.ProductCode = dto.ProductCode;
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.ImageUrl = dto.ImageUrl;

        // Update categories
        product.Categories.Clear();

        foreach (var catId in dto.CategoryIds)
        {
            var cat = await _categoryRepo.GetByIdAsync(catId);

            if (cat == null)
                throw new Exception($"Category with ID {catId} not found.");

            product.Categories.Add(cat);
        }

        _productRepo.Update(product);
        await _productRepo.SaveChangesAsync();

        return true;
    }

    private ProductResponseDto MapToResponse(Product p)
    {
        return new ProductResponseDto
        {
            ProductId = p.ProductId,
            ProductCode = p.ProductCode,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            ImageUrl = p.ImageUrl,

            AvailabilityStatusId = p.AvailabilityStatusId,
            AvailabilityStatusName = p.AvailabilityStatus?.Name,

            Stock = p.Inventory?.Stock ?? 0,

            Categories = p.Categories?
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToList()
                ?? new List<CategoryResponseDto>()
        };
    }
}