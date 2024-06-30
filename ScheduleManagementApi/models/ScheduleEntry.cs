using MongoDB.Bson.Serialization.Attributes;
using ScheduleManagementApi.models;

public class ScheduleEntry
{
    [BsonElement("startTime")]
    public DateTime StartTime { get; set; }

    [BsonElement("endTime")]
    public DateTime EndTime { get; set; }

    [BsonElement("mode")]
    public Mode? Mode { get; set; }

    [BsonElement("minValue")]
    public decimal MinValue { get; set; }

    [BsonElement("maxValue")]
    public decimal MaxValue { get; set; }

    [BsonElement("priority")] public Priority Priority { get; set; } = Priority.Default;
}