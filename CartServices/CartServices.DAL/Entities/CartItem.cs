using System;

namespace CartServices.DAL.Entities
{
    public class CartItem
    {
        public long CartItemId { get; set; }

        public long CartId { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public Cart Cart { get; set; } = null!;
    }
}
