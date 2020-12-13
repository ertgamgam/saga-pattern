using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace KafkaBroker
{
    public class KafkaMessageProducer : IKafkaMessageProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public KafkaMessageProducer(KafkaProducerConfiguration configuration)
        {
            var kafkaHost = configuration.KafkaHost;
            var producerConfig = new ProducerConfig() {BootstrapServers = kafkaHost};
            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = {new JsonStringEnumConverter()},
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task Produce(string topicName, string key, object message)
        {
            await _producer.ProduceAsync(topicName, new Message<string, string>
            {
                Key = key,
                Value = JsonSerializer.Serialize(message, _jsonSerializerOptions)
            });

            _producer.Flush();
        }
    }
}