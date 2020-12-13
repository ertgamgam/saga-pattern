using System;
using System.Threading.Tasks;
using KafkaBroker;
using Microsoft.Extensions.Logging;
using Stock.Repository;

namespace Stock.Consumer.OrderCreated
{
    public class OrderCreatedHandler : IHandler<OrderCreatedMessage>
    {
        private readonly StockDbContext _dbContext;
        private readonly ILogger<OrderCreatedHandler> _logger;

        public OrderCreatedHandler(StockDbContext dbContext, ILogger<OrderCreatedHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(OrderCreatedMessage message)
        {
            Console.WriteLine(message);
        }
    }
}