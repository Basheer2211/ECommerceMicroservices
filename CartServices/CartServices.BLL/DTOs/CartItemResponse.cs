namespace CartServices.BLL.DTOs
{
    public class CartItemResponse
    {
        public long ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }
    }
}
