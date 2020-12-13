using Microsoft.Extensions.DependencyInjection;
using Wallet.Repository;

namespace Wallet
{
    public static class TestDataExtensions
    {
        public static IServiceCollection AddTestWallet(this IServiceCollection services)
        {
            var stockDbContext = services.BuildServiceProvider().GetRequiredService<WalletDbContext>();
            stockDbContext.Wallets.AddRange(
                new Entity.Wallet {UserId = 333, Balance = 3000},
                new Entity.Wallet {UserId = 444, Balance = 5}
            );

            stockDbContext.SaveChanges();
            return services;
        }
    }
}