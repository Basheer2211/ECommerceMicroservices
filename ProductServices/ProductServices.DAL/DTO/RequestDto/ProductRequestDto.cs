using System.Collections.Generic;

namespace ProductServices.DAL.DTO.RequestDto;

public class ProductRequestDto
{
    public long ProductCode { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }


    public List<long> CategoryIds { get; set; } = new List<long>();
}
