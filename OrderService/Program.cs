using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OrderService
{
    class Program
    {
        private const string KafkaHost = "";
        private const string OrderCreatedTopic = "order-created";

        static async Task Main(string[] args)
        {
            var producerConfig = new ProducerConfig() {BootstrapServers = KafkaHost};
            var producer = new ProducerBuilder<string, string>(producerConfig).Build();

            var order = new Order();
            var orderSerialized = JsonSerializer.Serialize(order);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var deliveryResult = await producer.ProduceAsync(OrderCreatedTopic, new Message<string, string>
            {
                Key = order.Id,
                Value = orderSerialized
            }, cancellationToken);

            producer.Flush(cancellationToken);
        }
    }
}