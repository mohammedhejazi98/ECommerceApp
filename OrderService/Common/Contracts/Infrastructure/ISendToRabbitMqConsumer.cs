using OrderService.Data;
using OrderService.Entities;
using OrderService.Services.ServiceBus;
using System.Text.Json;

namespace OrderService.Common.Contracts.Infrastructure
{
    /// <summary>
    /// Interface representing the contract for sending messages to RabbitMQ for consumer processing.
    /// </summary>
    public interface ISendToRabbitMqConsumer
    {
        #region Methods

        /// <summary>
        /// Sends a message to RabbitMQ consumer and updates the consumer's database.
        /// The method ensures that the message is logged in the outbox for eventual consistency and then publishes the message to RabbitMQ.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="entityName">The name of the entity related to the message.</param>
        /// <param name="referenceId">The reference ID to correlate the message.</param>
        /// <param name="appId">The application ID that is sending the message.</param>
        /// <param name="exchange">The exchange name in RabbitMQ to publish the message.</param>
        /// <param name="routKey">The routing key in RabbitMQ to route the message.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</returns>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ProceedSendToConsumersDb(string message, string entityName, string referenceId, string appId,
            string exchange, string routKey, CancellationToken cancellationToken = default);

        #endregion Methods
    }


    /// <summary>
    /// The SendToRabbitMqConsumer class is responsible for handling message dispatch operations to RabbitMQ consumers.
    /// </summary>
    /// This class provides the functionality to send messages to a consumer database and to a RabbitMQ exchange.
    /// It utilizes the DataContext for database operations and IMessagesServiceBus for message bus interactions.
    /// <remarks>
    /// This class implements the ISendToRabbitMqConsumer interface.
    /// </remarks>
    public class SendToRabbitMqConsumer(DataContext dbContext, IMessagesServiceBus messagesServiceBus)
        : ISendToRabbitMqConsumer
    {
        #region Methods

        /// <summary>
        /// Sends a message to RabbitMQ consumers and stores the message in the database before proceeding.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="entityName">The name of the entity related to the message.</param>
        /// <param name="referenceId">The reference identifier for the message.</param>
        /// <param name="appId">The application identifier.</param>
        /// <param name="exchange">The RabbitMQ exchange to which the message will be sent.</param>
        /// <param name="routKey">The RabbitMQ routing key to be used.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ProceedSendToConsumersDb(string message, string entityName, string referenceId, string appId,
            string exchange, string routKey, CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> headers = new();

            var outboxId = Guid.NewGuid();
            await dbContext.Outbox.AddAsync(new Outbox
            {
                Id = outboxId,
                CreatedOn = DateTime.Now,
                Exchange = exchange,
                RoutKey = routKey,
                Message = message,
                Headers = JsonSerializer.Serialize(headers),
                Completed = false,
                AppId = nameof(OrderService),
                EntityName = entityName,
                ReferenceId = referenceId
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            messagesServiceBus.PublishToConsumerDb(message, exchange, routKey, headers);
        }

        #endregion Methods
    }
}