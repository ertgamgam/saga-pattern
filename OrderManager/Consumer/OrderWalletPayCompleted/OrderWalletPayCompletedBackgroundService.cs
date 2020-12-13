using KafkaBroker;

namespace OrderManager.Consumer.OrderWalletPayCompleted
{
    public class OrderWalletPayCompletedBackgroundService : KafkaConsumerBackgroundService<
        OrderWalletPayCompletedHandler,
        OrderWalletPayCompletedMessage>
    {
        public OrderWalletPayCompletedBackgroundService(OrderWalletPayCompletedHandler handler,
            KafkaConsumerConfiguration configuration) : base(handler, configuration)
        {
        }
    }
}