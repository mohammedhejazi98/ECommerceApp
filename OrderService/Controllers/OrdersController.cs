using Microsoft.AspNetCore.Mvc;

using OrderService.Common.Contracts.Infrastructure;
using OrderService.Dto;
using OrderService.Entities;

using System.Text.Json;
using OrderService.Data;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(ISendToRabbitMqConsumer rabbitMqConsumer,DataContext dataContext) : ControllerBase
    {
        #region Public Methods

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderItem orderItem)
        {
            var message = JsonSerializer.Serialize(orderItem);

            var order = new Order
            {
                Id = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity
            };
            await dataContext.Orders.AddAsync(order);
            await dataContext.SaveChangesAsync();
            
            await rabbitMqConsumer.ProceedSendToConsumersDb(message, nameof(Order), orderItem.OrderId.ToString(),
                nameof(OrderService), "OrderEvents", "OrderCreated");
            
            
            return Ok("Order Placed Successfully");
        }

        #endregion Public Methods
    }
}
