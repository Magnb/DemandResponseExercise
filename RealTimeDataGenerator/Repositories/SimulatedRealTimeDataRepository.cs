using ScheduleManagementApi.models;

namespace RealTimeDataGenerator;

public class SimulatedRealTimeDataRepository(ILogger<SimulatedRealTimeDataRepository> logger) : IRealTimeDataRepository
{
    public decimal GetDeviceRealTimeValue(ConsumerConfiguration deviceConfig)
    {
        var curTime = DateTime.UtcNow;
        var curInterval = deviceConfig.Schedule.Find(entry => curTime >= entry.StartTime && entry.EndTime >= curTime);
        if (curInterval != null)
        {
            logger.LogInformation("Generating Value from schedule interval {Min} - {Max}", curInterval.MinValue,
                curInterval.MaxValue);
            return Convert.ToDecimal(Random.Shared.NextDouble()) * (curInterval.MaxValue - curInterval.MinValue) +
                   curInterval.MinValue;
        }

        logger.LogInformation("Generating from default schedule");
        return Convert.ToDecimal(Random.Shared.NextDouble()) *
               (deviceConfig.DefaultMaxValue - deviceConfig.DefaultMinValue) +
               deviceConfig.DefaultMinValue;
    }
}