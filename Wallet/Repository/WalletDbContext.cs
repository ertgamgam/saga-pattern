using Microsoft.EntityFrameworkCore;

namespace Wallet.Repository
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
        {
        }

        public DbSet<Entity.Wallet> Products { get; set; }
    }
}