using System.Linq;
using System.Threading.Tasks;
using KafkaBroker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wallet.Repository;

namespace Wallet.Consumer.OrderStockUpdated
{
    public class OrderStockUpdatedHandler : IHandler<OrderStockUpdatedMessage>
    {
        private const string OrderWalletPayCompletedTopicName = "order-wallet-pay-completed";
        private const string OrderWalletPayErrorTopicName = "order-wallet-pay-error";
        private readonly WalletDbContext _dbContext;
        private readonly ILogger<OrderStockUpdatedHandler> _logger;
        private readonly IKafkaMessageProducer _kafkaMessageProducer;

        public OrderStockUpdatedHandler(WalletDbContext dbContext, ILogger<OrderStockUpdatedHandler> logger,
            IKafkaMessageProducer kafkaMessageProducer)
        {
            _dbContext = dbContext;
            _logger = logger;
            _kafkaMessageProducer = kafkaMessageProducer;
        }

        public async Task Handle(OrderStockUpdatedMessage message)
        {
            var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == message.UserId);
            var totalPrice = message.Products.Sum(x => x.Quantity * x.UnitPrice);
            if (wallet.Balance >= totalPrice)
            {
                wallet.Balance -= totalPrice;
                await _dbContext.SaveChangesAsync();
                await _kafkaMessageProducer.Produce(OrderWalletPayCompletedTopicName, message.OrderId.ToString(),
                    new {message.OrderId});
            }
            else
            {
                var orderWalletPayErrorMessage = new OrderWalletPayErrorMessage
                {
                    OrderId = message.OrderId,
                    WalletError = WalletError.InsufficientBalance,
                    Products = message.Products.Select(x => new ProductDto
                        {ProductId = x.ProductId, Quantity = x.Quantity})
                };

                await _kafkaMessageProducer.Produce(OrderWalletPayErrorTopicName, message.OrderId.ToString(),
                    orderWalletPayErrorMessage);
            }
        }
    }
}