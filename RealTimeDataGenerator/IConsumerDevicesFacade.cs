using ScheduleManagementApi.models;

namespace RealTimeDataGenerator;

public interface IConsumerDevicesFacade
{
    Task<IEnumerable<ConsumerConfiguration>> GetAllConsumerConfigs();
}