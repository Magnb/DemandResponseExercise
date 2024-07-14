using ScheduleManagementApi.models;

namespace RealTimeDataGenerator;

public interface IRealTimeDataRepository
{
    decimal GetDeviceRealTimeValue(ConsumerConfiguration deviceConfig);
}