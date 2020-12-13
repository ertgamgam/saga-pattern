using KafkaBroker;

namespace OrderManager.Consumer.OrderStockError
{
    public class
        OrderStockErrorBackgroundService : KafkaConsumerBackgroundService<OrderStockErrorHandler,
            OrderStockUpdateErrorMessage>
    {
        public OrderStockErrorBackgroundService(OrderStockErrorHandler handler,
            KafkaConsumerConfiguration configuration) :
            base(handler, configuration)
        {
        }
    }
}