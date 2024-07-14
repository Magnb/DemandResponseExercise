namespace ScheduleManagementApi.models.DTO;

public class ScheduleDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Mode? Mode { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public Priority? Priority { get; set; }
}