using Microsoft.EntityFrameworkCore;
using Stock.Entity;

namespace Stock.Repository
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}