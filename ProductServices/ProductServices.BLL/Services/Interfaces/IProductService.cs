using Microsoft.AspNetCore.Http;
using ProductServices.DAL.DTO.RequestDto;
using ProductServices.DAL.DTO.ResponseDto;

namespace ProductServices.BLL.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<ProductResponseDto?> GetByIdAsync(long id);
    Task<ProductResponseDto> CreateAsync(ProductRequestDto dto);
    Task<bool> UpdateAsync(long id, ProductRequestDto dto);
    Task<bool> DeleteAsync(long id);
    Task<ProductResponseDto?> GetByProductCodeAsync(long productCode);
     Task<IEnumerable<ProductResponseDto>> GetByPriceRangeAsync(
    decimal price1,
    decimal price2);
    Task<IEnumerable<ProductResponseDto>> GetByCategoryNameAsync(
   string categoryName);

    /// <summary>
    /// Import products in bulk. Returns (true, null) on success or (false, errorMessage) on failure.
    /// </summary>


}
