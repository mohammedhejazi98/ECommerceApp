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

    }
}