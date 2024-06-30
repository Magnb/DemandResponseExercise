namespace ScheduleManagementApi.models;

public class WallboxConfiguration : ConsumerConfiguration
{
    public WallboxConfiguration() : base("Wallbox", "Power", "Watt", Mode.Idle, 0, 0)
    {
    }
}