using ProductServices.DAL.DTO.RequestDto;
using ProductServices.DAL.DTO.ResponseDto;

namespace ProductServices.BLL.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    Task<CategoryResponseDto?> GetByIdAsync(long id);
    Task<CategoryResponseDto> CreateAsync(CategoryRequestDto dto);
    Task<bool> UpdateAsync(long id, CategoryRequestDto dto);
    Task<bool> DeleteAsync(long id);

}
