namespace InventoryService.Entities
{
    /// <summary>
    /// Represents an outbox entity used for storing messages that need to be sent to a message broker like RabbitMQ.
    /// </summary>
    /// <remarks>
    /// This class contains properties necessary for tracking and sending messages including the message details, routing information, and status.
    /// </remarks>
    public class Outbox 
    {
        #region Properties

        /// <summary>
        /// Gets or sets the date and time when the outbox message was created.
        /// </summary>
        /// <remarks>
        /// This property is used to track the creation timestamp of the outbox entry.
        /// </remarks>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the exchange to which the message will be sent.
        /// </summary>
        /// <remarks>
        /// The exchange is a part of the message broker system where messages are sent before being routed to a queue.
        /// This property is used to specify the target exchange for outbound messages in the outbox pattern.
        /// </remarks>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the Outbox entity.
        /// </summary>
        /// <remarks>
        /// The Id property is a globally unique identifier (GUID) assigned to each Outbox instance.
        /// It is used to uniquely identify a particular Outbox message in the system.
        /// </remarks>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content of the message to be sent or processed.
        /// </summary>
        /// <remarks>
        /// This property holds the main payload or data that needs to be transmitted
        /// or processed by the outbox entity. It is typically serialized in a string
        /// format and may contain any relevant information necessary for the message
        /// transaction within the InventoryService system.
        /// </remarks>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the routing key used for message delivery in the RabbitMQ messaging system.
        /// </summary>
        /// <remarks>
        /// The routing key is utilized by RabbitMQ to determine which queue(s) will receive the message.
        /// </remarks>
        public string RoutKey { get; set; }

        /// <summary>
        /// Gets or sets a serialized string representing a collection of headers associated with the message.
        /// </summary>
        /// <remarks>
        /// The Headers property contains additional metadata in the form of key-value pairs, serialized to a JSON string.
        /// This information is used by the messaging infrastructure to route, filter, or otherwise process the message.
        /// </remarks>
        public string Headers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the outbox message has been successfully processed.
        /// </summary>
        /// <value>
        /// A boolean value where true indicates that the message has been processed successfully, and false indicates that the processing is incomplete or failed.
        /// </value>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the application identifier associated with the outbox entry.
        /// </summary>
        /// <remarks>
        /// This property is used to specify the application origin from which the message is sent.
        /// </remarks>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier used to reference an external entity related to the outbox entry.
        /// </summary>
        /// <remarks>
        /// This property is used to associate the outbox entry with a specific entity in an external system,
        /// ensuring traceability and correlation between systems.
        /// </remarks>
        public string ReferenceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity related to the outbox message.
        /// </summary>
        /// <remarks>
        /// This property is used to identify the specific entity type that is associated with the message stored in the outbox.
        /// </remarks>
        public string EntityName { get; set; }
        #endregion Properties
    }
}
