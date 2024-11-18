using InventoryService.Entities;

using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data
{
    /// <summary>
    /// A data context class representing the database context for the InventoryService application.
    /// </summary>
    /// <remarks>
    /// This class inherits from the DbContext class provided by Entity Framework Core and is used to interact with the underlying database.
    /// </remarks>
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        #region Public Properties

        /// <summary>
        /// Represents an entity used to store messages for eventual transfer to a message queue or other service.
        /// </summary>
        /// <remarks>
        /// The Outbox pattern is implemented to ensure reliable message delivery. Messages are first stored in the database
        /// before being sent to the message queue, allowing for retry in case of failure.
        /// </remarks>
        public DbSet<Outbox> Outbox { get; set; }

        /// <summary>
        /// Represents the collection of products in the inventory.
        /// </summary>
        /// <remarks>
        /// Utilized within the <c>DataContext</c> class, this property provides access
        /// to the <c>Product</c> entities stored in the database through Entity Framework Core.
        /// </remarks>
        public DbSet<Product> Products { get; set; }

        #endregion Public Properties

        /// Configures the model and relationships using the provided ModelBuilder.
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Product>().HasData(

                      new Product()
                      {
                          AvailableQuantity = 5,
                          Name = "Iphone 15",
                          Price = 600,
                          Id = 1
                      },
                      new Product()
                      {
                          AvailableQuantity = 3,
                          Name = "Iphone 16",
                          Price = 900,
                          Id = 2

                      },
                      new Product()
                      {
                          AvailableQuantity = 1,
                          Name = "Samsung s23 ultra",
                          Price = 780,
                          Id = 3

                      }

              );
        }

    }
}