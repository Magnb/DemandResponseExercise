namespace MonitoringApp.models;

public class AppConfiguration
{
    
    public string ScheduleManagementApiUri { get; set; }
    public InfluxDbConfig InfluxDB { get; set; }
}