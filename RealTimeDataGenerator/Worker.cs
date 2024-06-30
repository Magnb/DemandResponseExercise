using Confluent.Kafka;
using RealTimeDataGenerator.models;

namespace RealTimeDataGenerator;

public class Worker(ILogger<Worker> logger, WorkerConfig configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var i = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await PublishToKafkaAsync(await SimulateCurrentValueAsync(i), "meter1", stoppingToken);        
            await Task.Delay(configuration.DataIntervalInMs, stoppingToken);
            i++;
        }
    }

    private async Task<int> SimulateCurrentValueAsync(int iteration)
    {
        // TODO get min und max from current schedule from database
        await Task.CompletedTask;
        if (iteration < configuration.ChangeIntervalAfterIterations)
        {
            return Random.Shared.Next(configuration.MinValue, configuration.MaxValue);
        }

        if (iteration < configuration.ChangeIntervalAfterIterations * 2)
        {
            return 0;
        }

        if (iteration < configuration.ChangeIntervalAfterIterations * 3)
        {
            return Random.Shared.Next(configuration.MinValue * 3, configuration.MaxValue * 3);
        }
        return Random.Shared.Next(configuration.MinValue, configuration.MaxValue);
    }

    private async Task PublishToKafkaAsync(decimal value, string device, CancellationToken stoppingToken)
    {
        var producer = new ProducerBuilder<string, string>(configuration.Kafka.ProducerConfig).Build();
        var delivery = producer.ProduceAsync(configuration.Kafka.Topic, new Message<string, string> { Value = value.ToString(), Key = device}, stoppingToken)
            .Result;
        logger.LogInformation("Sent value {Msg} to Topic {Topic} {deliveryRep}", value, configuration.Kafka.Topic, delivery.Message.Value);
    }
}