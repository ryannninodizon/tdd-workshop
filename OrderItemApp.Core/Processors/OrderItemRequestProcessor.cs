using OrderItemApp.Core.DataServices;
using OrderItemApp.Core.Domain;
using OrderItemApp.Core.Models;

namespace OrderItemApp.Core.Processors
{
    public class OrderItemRequestProcessor
    {
        private IOrderItemService _orderItemService;

        public OrderItemRequestProcessor(IOrderItemService orderItemService)
        {
            this._orderItemService = orderItemService;
        }

        public OrderItemResult OrderItem(OrderItemRequest itemRequest)
        {
            if (itemRequest is null)
            {
                throw new ArgumentNullException(nameof(itemRequest));
            }

            //_roomItemService.Save(new OrderItem
            //{
            //    FullName = itemRequest.FullName,
            //    Email = itemRequest.Email,
            //    Date = itemRequest.Date
            //});

            //return new OrderItemResult
            //{
            //    FullName = itemRequest.FullName,
            //    Email = itemRequest.Email,
            //    Date = itemRequest.Date
            //};
            var availableRooms = _orderItemService.GetAllAvailableItems(itemRequest.Date);
            if (availableRooms.Any())
            {
                _orderItemService.Save(CreateOrderItemObject<OrderItem>(itemRequest));
            }

            return CreateOrderItemObject<OrderItemResult>(itemRequest);
        }

        private static TOrderItem CreateOrderItemObject<TOrderItem>(OrderItemRequest itemRequest) where TOrderItem : OrderItemBase, new()
        {
            return new TOrderItem
            {
                FullName = itemRequest.FullName,
                Email = itemRequest.Email,
                Date = itemRequest.Date
            };
        }
    }
}