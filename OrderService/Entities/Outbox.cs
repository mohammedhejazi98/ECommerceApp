namespace OrderService.Entities
{
    /// <summary>
    /// Represents an outbox message that stores information about messages to be sent to RabbitMQ.
    /// </summary>
    public class Outbox 
    {
        #region Properties

        /// <summary>
        /// Gets or sets the timestamp when the outbox message was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Defines the exchange to which the message should be sent.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the outbox entry.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The body of the message that is published to the consumer database via the messaging service bus.
        /// This property holds the main content of the message that needs to be transmitted.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the routing key for the message.
        /// </summary>
        /// <remarks>
        /// This property specifies the routing key used to route the message within the messaging system.
        /// </remarks>
        public string RoutKey { get; set; }

        /// <summary>
        /// Gets or sets the headers associated with the outbox message.
        /// The headers provide additional metadata in key-value pairs that can be useful
        /// for message processing and routing within the messaging infrastructure.
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the outbox message has been completed.
        /// </summary>
        /// <remarks>
        /// This property is used to track the processing status of outbox messages.
        /// A value of <c>false</c> indicates that the message has not yet been processed,
        /// while a value of <c>true</c> indicates that it has been successfully processed.
        /// </remarks>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the application identifier associated with the outbox message.
        /// This property is used to identify the application that originated the message.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier associated with the entity in the outbox.
        /// This identifier is used to reference the associated record in external systems or databases.
        /// </summary>
        public string ReferenceId { get; set; }

        /// <summary>
        /// Specifies the name of the entity associated with this outbox entry.
        /// </summary>
        public string EntityName { get; set; }
        #endregion Properties
    }
}
