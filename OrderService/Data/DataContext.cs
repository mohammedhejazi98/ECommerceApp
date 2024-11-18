using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.Data
{
    /// <summary>
    /// Represents the database context for the OrderService application, which is derived from the Entity Framework's DbContext.
    /// </summary>
    /// <remarks>
    /// The DataContext class provides properties to access the database entities like Outbox and Orders.
    /// It also facilitates the configuration and connectivity to the SQL Server database using the connection string
    /// defined in the application configuration.
    /// </remarks>
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        #region Public Properties

        /// <summary>
        /// Represents an outbox entity used for storing messages that are intended to be sent to message queues or other external systems.
        /// </summary>
        /// <remarks>
        /// This entity is commonly used to implement the Outbox pattern, which ensures that messages are reliably sent to external systems
        /// even in the presence of system failures. Each outbox entry contains information about the message, routing details, and the status
        /// of the message delivery.
        /// </remarks>
        public DbSet<Outbox> Outbox { get; set; }

        /// <summary>
        /// Gets or sets the collection of Orders.
        /// </summary>
        /// <value>
        /// A DbSet of Order entities representing the orders stored in the database.
        /// </value>
        public DbSet<Order> Orders { get; set; }

        #endregion Public Properties

    }
}