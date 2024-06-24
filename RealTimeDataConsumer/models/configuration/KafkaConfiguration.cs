using Confluent.Kafka;

namespace RealTimeDataConsumer.models.configuration;

public class KafkaConfiguration
{
    public ConsumerConfig ConsumerConfig { get; set; }
    public string Topic { get; set; }
}