namespace ScheduleManagementApi.models;

public class HeatpumpConfiguration : ConsumerConfiguration
{
    public HeatpumpConfiguration() : base("Heatpump", "Temperature", "Celsius", Mode.On, 21.5M, 22.2M)
    {
    }
}