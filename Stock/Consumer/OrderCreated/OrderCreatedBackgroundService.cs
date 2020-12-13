using KafkaBroker;

namespace Stock.Consumer.OrderCreated
{
    public class
        OrderCreatedBackgroundService : KafkaConsumerBackgroundService<OrderCreatedHandler, OrderCreatedMessage>
    {
        public OrderCreatedBackgroundService(OrderCreatedHandler handler, KafkaConsumerConfiguration configuration) :
            base(handler, configuration)
        {
        }
    }
}