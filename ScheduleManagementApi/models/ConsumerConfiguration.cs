using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ScheduleManagementApi.models;

public class ConsumerConfiguration
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("deviceName")]
    public string? DeviceName { get; set; }

    [BsonElement("deviceCategory")]
    public string DeviceCategory { get; protected set; }

    [BsonElement("sensorCategory")]
    public string SensorCategory { get; protected set; }

    [BsonElement("sensorUnit")]
    public string SensorUnit { get; protected set; }

    [BsonElement("defaultMinValue")]
    public decimal DefaultMinValue { get; set; }

    [BsonElement("defaultMaxValue")]
    public decimal DefaultMaxValue { get; set; }

    [BsonElement("defaultMode")]
    public Mode DefaultMode { get; protected set; }
    
    [BsonElement("schedule")]
    public List<ScheduleEntry> Schedule { get; set; } = [];

    [BsonElement("conditions")]
    public List<Condition> Conditions { get; set; } = [];

    protected ConsumerConfiguration(string deviceCategory, string sensorCategory, string sensorUnit, Mode mode, decimal defaultMin, decimal defaultMax)
    {
        DeviceCategory = deviceCategory;
        SensorCategory = sensorCategory;
        SensorUnit = sensorUnit;
        DefaultMode = mode;
        DefaultMinValue = defaultMin;
        DefaultMaxValue = defaultMax;
    }
}