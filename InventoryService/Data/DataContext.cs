using InventoryService.Entities;

using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data
{

    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        #region Public Properties

        public DbSet<Outbox> Outbox { get; set; }
        public DbSet<Product> Products { get; set; }

        #endregion Public Properties
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