using KafkaBroker;

namespace Stock.Consumer.OrderWalletPayError
{
    public class
        OrderWalletPayErrorBackgroundService : KafkaConsumerBackgroundService<OrderWalletPayErrorHandler,
            OrderWalletPayErrorMessage>
    {
        public OrderWalletPayErrorBackgroundService(OrderWalletPayErrorHandler handler,
            KafkaConsumerConfiguration configuration) : base(handler, configuration)
        {
        }
    }
}