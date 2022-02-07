using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quentity { get; set; }
        public decimal Price { get; set; }
        public int DiscountRate { get; set; }
        public decimal DiscountedPrice => Price - (Price * DiscountRate / 100.0m);
        public decimal Amount => DiscountedPrice * Quentity;
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
