using Confluent.Kafka;

namespace RealTimeDataGenerator.models;

public class KafkaConfiguration
{
    public ProducerConfig ProducerConfig { get; set; }
    public string Topic { get; set; }
}