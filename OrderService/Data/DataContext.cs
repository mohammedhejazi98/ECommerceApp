using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.Data
{

    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        #region Public Properties

        public DbSet<Outbox> Outbox { get; set; }
        public DbSet<Order> Orders { get; set; }

        #endregion Public Properties

    }
}