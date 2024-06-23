namespace RealTimeDataGenerator.models;

public class WorkerConfig
{
    public KafkaConfiguration Kafka { get; set; }
    public IEnumerable<SimulationConfig> Simulations { get; set; }
    public int DataIntervalInMs { get; set; }
}