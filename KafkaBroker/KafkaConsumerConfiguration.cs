namespace KafkaBroker
{
    public class KafkaConsumerConfiguration
    {
        public string KafkaHost { get; set; }
        public string TopicName { get; set; }
        public string GroupId { get; set; }
    }
}