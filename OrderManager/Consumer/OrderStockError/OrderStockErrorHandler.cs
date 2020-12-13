using System.Threading.Tasks;
using KafkaBroker;
using Microsoft.EntityFrameworkCore;
using OrderManager.Entity;
using OrderManager.Repository;

namespace OrderManager.Consumer.OrderStockError
{
    public class OrderStockErrorHandler : IHandler<OrderStockUpdateErrorMessage>
    {
        private readonly OrderDbContext _dbContext;

        public OrderStockErrorHandler(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(OrderStockUpdateErrorMessage message)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(x => x.Id == message.OrderId);

            order.Status = OrderStatus.Failed;
            order.FailCause = message.StockUpdateError;

            await _dbContext.SaveChangesAsync();
        }
    }
}