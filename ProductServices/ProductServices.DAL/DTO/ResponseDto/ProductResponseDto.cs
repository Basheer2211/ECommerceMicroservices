using System.Collections.Generic;

namespace ProductServices.DAL.DTO.ResponseDto;

public class ProductResponseDto
{
    public long ProductId { get; set; }
    public long ProductCode { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int AvailabilityStatusId { get; set; }
    public string? AvailabilityStatusName { get; set; }
    public int Stock { get; set; }
    public List<CategoryResponseDto> Categories { get; set; } = new List<CategoryResponseDto>();
}
