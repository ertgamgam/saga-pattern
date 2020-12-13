using KafkaBroker;

namespace Wallet.Consumer.OrderStockUpdated
{
    public class
        OrderStockUpdatedBackgroundService : KafkaConsumerBackgroundService<OrderStockUpdatedHandler,
            OrderStockUpdatedMessage>
    {
        public OrderStockUpdatedBackgroundService(OrderStockUpdatedHandler handler,
            KafkaConsumerConfiguration configuration) : base(handler, configuration)
        {
        }
    }
}