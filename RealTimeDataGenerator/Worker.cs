using Confluent.Kafka;
using RealTimeDataGenerator.models;

namespace RealTimeDataGenerator;

public class Worker(
    ILogger<Worker> logger,
    WorkerConfig configuration,
    IConsumerDevicesFacade consumerDevicesFacade,
    IRealTimeDataRepository realTimeDataRepo) : BackgroundService
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
                await PublishToKafkaAsync(realTimeDataRepo.GetDeviceRealTimeValue(deviceConfig),
                    deviceConfig.Id.ToString() ?? deviceConfig.DeviceCategory, stoppingToken);
            }

            await Task.Delay(configuration.FetchDataIntervalInMs, stoppingToken);
        }
    }

    private async Task PublishToKafkaAsync(decimal value, string device, CancellationToken stoppingToken)
    {
        var producer = new ProducerBuilder<string, string>(configuration.Kafka.ProducerConfig).Build();
        var delivery = producer.ProduceAsync(configuration.Kafka.Topic,
                new Message<string, string> { Value = value.ToString(), Key = device }, stoppingToken)
            .Result;
        logger.LogInformation("Sent value {Msg} to Topic {Topic} {deliveryRep}", value, configuration.Kafka.Topic,
            delivery.Message.Value);
    }
}