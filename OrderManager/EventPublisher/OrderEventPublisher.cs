using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderManager.Entity;

namespace OrderManager.EventPublisher
{
    public class OrderEventPublisher : IOrderEventPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _orderCreatedTopicName;
        private readonly ILogger<OrderEventPublisher> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public OrderEventPublisher(IConfiguration configuration, ILogger<OrderEventPublisher> logger)
        {
            _logger = logger;
            var kafkaHost = configuration.GetValue<string>("KafkaHost");
            _orderCreatedTopicName = configuration.GetValue<string>("OrderCreatedTopic");
            var producerConfig = new ProducerConfig() {BootstrapServers = kafkaHost};
            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = {new JsonStringEnumConverter()},
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task PublishOrderCreateEvent(Order order)
        {
            var deliveryResult = await _producer.ProduceAsync(_orderCreatedTopicName, new Message<string, string>
            {
                Key = order.Id.ToString(),
                Value = JsonSerializer.Serialize(order, _jsonSerializerOptions)
            });

            _producer.Flush();
            _logger.LogInformation($"Order {order.Id} delivery result = {deliveryResult.Status} ");
        }
    }
}