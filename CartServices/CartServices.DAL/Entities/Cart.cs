using System;
using System.Collections.Generic;

namespace CartServices.DAL.Entities
{
    public class Cart
    {
        public long CartId { get; set; }

        public long CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;

        public string CustomerAddress { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
