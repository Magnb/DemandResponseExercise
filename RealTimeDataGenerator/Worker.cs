using Confluent.Kafka;
using RealTimeDataGenerator.models;
using ScheduleManagementApi.models;

namespace RealTimeDataGenerator;

public class Worker(ILogger<Worker> logger, WorkerConfig configuration, IHttpClientFactory httpClientFactory, IConsumerDevicesFacade consumerDevicesFacade) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogDebug("Worker running at: {time}", DateTimeOffset.Now);
            var devices = await consumerDevicesFacade.GetAllConsumerConfigs();
            logger.LogInformation("Generating data for {DevicesCount} devices", devices.Count());
            
            foreach (var deviceConfig in devices)
            {
                await PublishToKafkaAsync(SimulateCurrentValueAsync(deviceConfig), deviceConfig.DeviceName ?? deviceConfig.DeviceCategory, stoppingToken);       
            } 
            await Task.Delay(configuration.DataIntervalInMs, stoppingToken);
        }
    }
    
    private decimal SimulateCurrentValueAsync(ConsumerConfiguration device)
    {
        var curTime = DateTime.UtcNow;
        var curInterval = device.Schedule.Find(entry => entry.StartTime < curTime && entry.EndTime <= curTime);
        if (curInterval != null)
        {
            logger.LogInformation("Schedule interval {Min} - {Max}", curInterval.MinValue, curInterval.MaxValue);
            return Convert.ToDecimal(Random.Shared.NextDouble()) * (curInterval.MaxValue - curInterval.MinValue) +
                   curInterval.MinValue;
        }
        logger.LogInformation("ok default it is");
        return Convert.ToDecimal(Random.Shared.NextDouble()) * (device.DefaultMaxValue - device.DefaultMinValue) *
              device.DefaultMinValue;
    }
    
    private async Task PublishToKafkaAsync(decimal value, string device, CancellationToken stoppingToken)
    {
        var producer = new ProducerBuilder<string, string>(configuration.Kafka.ProducerConfig).Build();
        var delivery = producer.ProduceAsync(configuration.Kafka.Topic, new Message<string, string> { Value = value.ToString(), Key = device}, stoppingToken)
            .Result;
        logger.LogInformation("Sent value {Msg} to Topic {Topic} {deliveryRep}", value, configuration.Kafka.Topic, delivery.Message.Value);
    }
}