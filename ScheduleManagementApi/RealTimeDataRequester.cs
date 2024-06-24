using Confluent.Kafka;
using RealTimeDataConsumer.models.configuration;

namespace ScheduleManagementApi;

public class RealTimeDataRequester(ApiConfiguration configuration, ILogger<RealTimeDataRequester> logger)
{
    public void ConsumeMessages(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = configuration.Kafka.ConsumerConfig.GroupId,
            BootstrapServers = configuration.Kafka.ConsumerConfig.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(configuration.Kafka.Topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var cr = consumer.Consume(stoppingToken);
                logger.LogInformation("Consumed message '{Message}' at: '{TopicPartitionOffset}'.", cr.Message.Value,
                    cr.TopicPartitionOffset);

            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while consuming Kafka messages.");
        }
    }
}