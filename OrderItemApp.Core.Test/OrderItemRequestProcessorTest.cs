using Moq;
using OrderItemApp.Core.DataServices;
using OrderItemApp.Core.Domain;
using OrderItemApp.Core.Models;
using OrderItemApp.Core.Processors;
using Shouldly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderItemApp.Core
{
    public class OrderItemRequestProcessorTest
    {
        private OrderItemRequest _request;
        private OrderItemRequestProcessor _processor;
        private Mock<IOrderItemService> _orderItemServiceMock;
        private List<Order> _availableItems;

        public OrderItemRequestProcessorTest()
        {
            //Arrange
            _request = new OrderItemRequest
            {
                FullName = "Ryan",
                Email = "email",
                Date = new DateTime(2012, 02, 03)
            };
            _availableItems = new List<Order>() { new Order() };
            
            _orderItemServiceMock = new Mock<IOrderItemService>();
            _orderItemServiceMock.Setup(q => q.GetAllAvailableItems(_request.Date))
                .Returns(_availableItems);

            _processor = new OrderItemRequestProcessor(_orderItemServiceMock.Object);
        }
        [Fact]
        public void Should_Return_Order_Item_Response_With_Request_values()
        {
            //Arrange
            var request = new OrderItemRequest
            {
                FullName = "Ryan",
                Email = "email",
                Date = new DateTime(2012, 02, 03)
            };
            var processor = new OrderItemRequestProcessor(_orderItemServiceMock.Object);


            //Act
            OrderItemResult result = _processor.OrderItem(request);

            
            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.FullName, result.FullName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Date, result.Date);

            result.ShouldNotBeNull();
            result.FullName.ShouldBe(result.FullName);
            result.Email.ShouldBe(result.Email);
            result.Date.ShouldBe(result.Date);  
        }
        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            var processor = new OrderItemRequestProcessor(_orderItemServiceMock.Object);

            var exception = Should.Throw<ArgumentNullException>(() => _processor.OrderItem(null));

            exception.ParamName.ShouldBe("itemRequest");

        }
        [Fact]
        public void Should_Save_Order_Item_Request()
        {
            OrderItem savedItem = null;
            _orderItemServiceMock
                .Setup(q => q.Save(It.IsAny<OrderItem>()))
                .Callback<OrderItem>(Item =>
                {
                    savedItem = Item;
                });


            _processor.OrderItem(_request);

            _orderItemServiceMock.Verify(q => q.Save(It.IsAny<OrderItem>()), Times.Once);
            savedItem.ShouldNotBeNull();
            savedItem.FullName.ShouldBe(savedItem.FullName);
            savedItem.Email.ShouldBe(savedItem.Email);
            savedItem.Date.ShouldBe(savedItem.Date);

        }
        [Fact]
        public void Should_Not_Save_Order_Item_Request_If_None_Available()
        {
            _availableItems.Clear();
            _processor.OrderItem(_request);
            _orderItemServiceMock.Verify(q => q.Save(It.IsAny<OrderItem>()), Times.Never);
        }

        //[Theory]
        //[InlineData(OrderResultFlag.Failure, false)]
        //[InlineData(OrderResultFlag.Success, true)]
        //public void Should_Return_SuccessOrFailure_Flag_In_Result(OrderResultFlag ItemSuccessFlag, bool isAvailable)
        //{
        //    if (!isAvailable)
        //    {
        //        _availableOrders.Clear();
        //    }

        //    var result = _processor.OrderOrder(_request);
        //    ItemSuccessFlag.ShouldBe(result.Flag);

        //}
    }
}
