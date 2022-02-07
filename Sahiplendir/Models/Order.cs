using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public enum OrderStatus
    {
        New, Shipped, Cancelled
    }

    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingCode { get; set; }
        public decimal TotalAmount => OrderItems.Sum(p => p.Amount);
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

    }
}
