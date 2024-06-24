namespace RealTimeDataConsumer.models.configuration;

public class WorkerConfig
{
    public KafkaConfiguration Kafka { get; set; }
    
    public string SignalHubUri { get; set; }
}