using Microsoft.AspNetCore.Mvc;

using OrderService.Common.Contracts.Infrastructure;
using OrderService.Dto;
using OrderService.Entities;

using System.Text.Json;
using OrderService.Data;

namespace OrderService.Controllers
{
    /// <summary>
    /// OrdersController handles API requests related to order operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(ISendToRabbitMqConsumer rabbitMqConsumer,DataContext dataContext) : ControllerBase
    {
        #region Public Methods

        /// <summary>
        /// Creates a new order and sends a message to the RabbitMQ consumer for further processing.
        /// </summary>
        /// <param name="orderItem">An OrderItem object containing the details of the order to be created.</param>
        /// <returns>A response indicating the success or failure of the order creation process.</returns>
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
            
            // Sending order creation message to RabbitMQ consumer for further processing
            await rabbitMqConsumer.ProceedSendToConsumersDb(message, nameof(Order), orderItem.OrderId.ToString(),
                nameof(OrderService), "OrderEvents", "OrderCreated");
            
            
            return Ok("Order Placed Successfully");
        }

        #endregion Public Methods
    }
}
