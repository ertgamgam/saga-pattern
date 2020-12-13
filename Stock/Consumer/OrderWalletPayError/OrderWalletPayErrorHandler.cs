using System.Linq;
using System.Threading.Tasks;
using KafkaBroker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Repository;

namespace Stock.Consumer.OrderWalletPayError
{
    public class OrderWalletPayErrorHandler : IHandler<OrderWalletPayErrorMessage>
    {
        private readonly StockDbContext _dbContext;
        private readonly ILogger<OrderWalletPayErrorHandler> _logger;

        public OrderWalletPayErrorHandler(StockDbContext dbContext, ILogger<OrderWalletPayErrorHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(OrderWalletPayErrorMessage message)
        {
            _logger.LogWarning($"OrderWalletPayError message was received. Order Id = {message.OrderId}");
            var productIds = message.Products
                .Select(x => x.ProductId)
                .ToList();

            var dbProducts = await _dbContext.Products
                .Where(product => productIds.Contains(product.Id))
                .ToListAsync();

            dbProducts.ForEach(dbProduct =>
            {
                var quantity = message.Products
                    .FirstOrDefault(messageProduct => messageProduct.ProductId == dbProduct.Id)?
                    .Quantity ?? default;
                dbProduct.StockQuantity += quantity;
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}