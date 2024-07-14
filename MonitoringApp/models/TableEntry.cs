using System.Transactions;
using ScheduleManagementApi.models;

namespace MonitoringApp.models;

public class TableEntry: ConsumerConfiguration
{
    public decimal CurrentValue { get; set; }

    public TableEntry(ConsumerConfiguration consumer, decimal curValue)
    {
        Id = consumer.Id;
        DeviceCategory = consumer.DeviceCategory;
        DeviceName = consumer.DeviceName;
        SensorCategory = consumer.SensorCategory;
        SensorUnit = consumer.SensorUnit;
        DefaultMode = consumer.DefaultMode;
        Schedule = consumer.Schedule;
        Conditions = consumer.Conditions;
        
        CurrentValue = curValue;
    }
}