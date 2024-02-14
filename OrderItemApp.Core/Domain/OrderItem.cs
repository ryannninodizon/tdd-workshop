using OrderItemApp.Core.Models;

namespace OrderItemApp.Core.Domain
{
    public class OrderItem : OrderItemBase
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}