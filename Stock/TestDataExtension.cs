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
                new Product {Id = 1, Price = 10, StockQuantity = 1000},
                new Product {Id = 2, Price = 20, StockQuantity = 1000},
                new Product {Id = 3, Price = 30, StockQuantity = 2});

            stockDbContext.SaveChanges();
            return services;
        }
    }
}