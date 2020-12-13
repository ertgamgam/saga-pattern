using System.Threading.Tasks;
using KafkaBroker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManager.Entity;
using OrderManager.Repository;

namespace OrderManager.Consumer.OrderStockError
{
    public class OrderStockErrorHandler : IHandler<OrderStockUpdateErrorMessage>
    {
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<OrderStockErrorHandler> _logger;

        public OrderStockErrorHandler(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(OrderStockUpdateErrorMessage message)
        {
            _logger.LogInformation($"OrderStockUpdateError message was received. Order Id = {message.OrderId}");
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(x => x.Id == message.OrderId);

            order.Status = OrderStatus.Failed;
            order.FailCause = message.StockUpdateError;
            await _dbContext.SaveChangesAsync();
            _logger.LogWarning(
                $"Order status was changed as {order.Status}. FailCause = ${order.FailCause}. Order Id = {order.Id}");
        }
    }
}