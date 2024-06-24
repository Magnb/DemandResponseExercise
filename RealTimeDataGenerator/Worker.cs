using Confluent.Kafka;
using RealTimeDataGenerator.models;

namespace RealTimeDataGenerator;

public class Worker(ILogger<Worker> _logger, WorkerConfig configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            int value = Random.Shared.Next(200, 555);
            var producer = new ProducerBuilder<Null, string>(configuration.Kafka.ProducerConfig).Build();
            var delivery = producer.ProduceAsync(configuration.Kafka.Topic, new Message<Null, string> { Value = value.ToString() }, stoppingToken)
                .Result;
            _logger.LogInformation("Sent value {Msg} to Topic {Topic} {deliveryRep}", value, configuration.Kafka.Topic, delivery.Message.Value);
            await Task.Delay(configuration.DataIntervalInMs, stoppingToken);
        }
    }
}