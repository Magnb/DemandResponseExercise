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
            string msg = "Hello, Kafka!";
            var producer = new ProducerBuilder<Null, string>(configuration.Kafka.ProducerConfig).Build();
            var delivery = producer.ProduceAsync(configuration.Kafka.Topic, new Message<Null, string> { Value = msg }, stoppingToken)
                .Result;
            _logger.LogInformation("Produced message {Msg} to Topic {Topic} {deliveryRep}", msg, configuration.Kafka.Topic, delivery.Message);
            await Task.Delay(configuration.DataIntervalInMs, stoppingToken);
        }
    }
}