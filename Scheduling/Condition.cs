using MongoDB.Bson.Serialization.Attributes;

namespace ScheduleManagementApi.models;

public abstract class Condition
{
    [BsonElement("startTime")]
    public DateTime StartTime { get; set; }

    [BsonElement("endTime")]
    public DateTime EndTime { get; set; }

    [BsonElement("energyPriceThreshold")]
    public decimal EnergyPriceThreshold { get; set; }
}