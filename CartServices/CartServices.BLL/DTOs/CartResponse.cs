using System;
using System.Collections.Generic;

namespace CartServices.BLL.DTOs
{
    public class CartResponse
    {
        public long CartId { get; set; }

        public long CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;

        public string CustomerAddress { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();

        public decimal TotalAmount { get; set; }
    }
}
