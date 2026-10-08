using System.Threading.Tasks;

namespace CartServices.BLL.Interfaces
{
    public interface IProductClient
    {
        Task<ProductInfo?> GetProductAsync(long productId);
    }

    public class ProductInfo
    {
        public long ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
