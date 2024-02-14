using OrderItemApp.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderItemApp.Core.DataServices
{
    public interface IOrderItemService
    {
        void Save(OrderItem orderItem);
        IEnumerable<Order> GetAllAvailableItems(DateTime date);
    }
}
