using System.Threading.Tasks;
using KafkaBroker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManager.Entity;
using OrderManager.Repository;

namespace OrderManager.Consumer.OrderWalletPayCompleted
{
    public class OrderWalletPayCompletedHandler : IHandler<OrderWalletPayCompletedMessage>
    {
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<OrderWalletPayCompletedHandler> _logger;

        public OrderWalletPayCompletedHandler(OrderDbContext dbContext, ILogger<OrderWalletPayCompletedHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(OrderWalletPayCompletedMessage message)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == message.OrderId);
            order.Status = OrderStatus.Created;
            await _dbContext.SaveChangesAsync();
        }
    }
}