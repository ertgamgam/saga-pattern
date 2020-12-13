using System.Threading.Tasks;
using KafkaBroker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManager.Entity;
using OrderManager.Repository;

namespace OrderManager.Consumer.OrderWalletPayError
{
    public class OrderWalletPayErrorHandler : IHandler<OrderWalletPayErrorMessage>
    {
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<OrderWalletPayErrorHandler> _logger;

        public OrderWalletPayErrorHandler(OrderDbContext dbContext, ILogger<OrderWalletPayErrorHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(OrderWalletPayErrorMessage message)
        {
            _logger.LogInformation($"OrderWalletPayError message was received. Order Id = {message.OrderId}");
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == message.OrderId);
            order.Status = OrderStatus.Failed;
            order.FailCause = message.WalletError;
            await _dbContext.SaveChangesAsync();
            _logger.LogWarning(
                $"Order status was changed as {order.Status}. FailCause = ${order.FailCause}. Order Id = {order.Id}");
        }
    }
}