namespace RealTimeDataGenerator.models;

public class RealtimeDataResponse
{
    public int ID { get; set; }
    public string Device { get; set; }
    public string DeviceId { get; set; }
    public decimal Value { get; set; }
    public Unit Unit { get; set; }
}