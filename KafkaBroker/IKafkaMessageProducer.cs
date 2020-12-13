using System.Threading.Tasks;

namespace KafkaBroker
{
    public interface IKafkaMessageProducer
    {
        Task Produce(string topicName, string key, object message);
    }
}