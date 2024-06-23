namespace RealTimeDataGenerator.models;

public class SimulationConfig
{
    public Consumer Consumer { get; set; }
    public TimeSpan SendDataInterval { get; set; }
    public decimal MinWatt { get; set; }
    public decimal MaxWatt { get; set; }
    public int DevicesCount { get; set; }
}