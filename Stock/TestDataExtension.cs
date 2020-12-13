using Microsoft.Extensions.DependencyInjection;
using Stock.Entity;
using Stock.Repository;

namespace Stock
{
    public static class TestDataExtension
    {
        public static IServiceCollection AddTestProducts(this IServiceCollection services)
        {
            var stockDbContext = services.BuildServiceProvider().GetRequiredService<StockDbContext>();
            stockDbContext.Products.AddRange(
                new Product {Id = 1, Price = 100, StockQuantity = 1000},
                new Product {Id = 2, Price = 100, StockQuantity = 1000},
                new Product {Id = 3, Price = 100, StockQuantity = 1000});

            stockDbContext.SaveChanges();
            return services;
        }
    }
}