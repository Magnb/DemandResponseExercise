using Confluent.Kafka;
using RealTimeDataGenerator.models;

namespace RealTimeDataGenerator;

public class Worker(ILogger<Worker> logger, WorkerConfig configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await PublishToKafkaAsync(await SimulateCurrentValueAsync(), stoppingToken);
        }
    }

    private async Task<int> SimulateCurrentValueAsync()
    {
        // TODO get min und max from current schedule from database
        await Task.CompletedTask; 
        var value = Random.Shared.Next(configuration.MinValue, configuration.MaxValue);
        return value;
    }

    private async Task PublishToKafkaAsync(decimal value, CancellationToken stoppingToken)
    {
        var producer = new ProducerBuilder<Null, string>(configuration.Kafka.ProducerConfig).Build();
        var delivery = producer.ProduceAsync(configuration.Kafka.Topic, new Message<Null, string> { Value = value.ToString() }, stoppingToken)
            .Result;
        logger.LogInformation("Sent value {Msg} to Topic {Topic} {deliveryRep}", value, configuration.Kafka.Topic, delivery.Message.Value);
        await Task.Delay(configuration.DataIntervalInMs, stoppingToken);
    }
}