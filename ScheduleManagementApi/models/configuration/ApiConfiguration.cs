namespace RealTimeDataConsumer.models.configuration;

public class ApiConfiguration
{
    public KafkaConfiguration Kafka { get; set; }
    public MongoDBSettings MongoDB { get; set; }
}