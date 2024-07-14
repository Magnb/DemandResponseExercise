using System.Net.Http.Json;
using RealTimeDataGenerator.models;
using ScheduleManagementApi.models;

namespace RealTimeDataGenerator;

public class ConsumerDevicesFacade : IConsumerDevicesFacade
{
        private readonly HttpClient _httpClient;

        public ConsumerDevicesFacade(HttpClient httpClient, WorkerConfig config)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(config.ScheduleManagementApiUri);
        }

        public async Task<IEnumerable<ConsumerConfiguration>> GetAllConsumerConfigs() =>
            await _httpClient.GetFromJsonAsync<IEnumerable<ConsumerConfiguration>>("api/Consumers") ?? new List<ConsumerConfiguration>();
}