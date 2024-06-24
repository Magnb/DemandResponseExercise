using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR.Client;
using RealTimeDataConsumer.models.configuration;

namespace RealTimeDataConsumer;

public class Worker(ILogger<Worker> logger, WorkerConfig configuration) : BackgroundService
{
    private readonly HubConnection _hubConnection = new HubConnectionBuilder()
        .WithUrl(configuration.SignalHubUri)
        .Build();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _hubConnection.StartAsync(stoppingToken);
        logger.LogInformation("SignalR connection established.");
 
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

                await _hubConnection.SendAsync("SendMessage", cr.Message.Value, cancellationToken: stoppingToken);
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

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync(stoppingToken);
            await _hubConnection.DisposeAsync();
        }

        await base.StopAsync(stoppingToken);
    }
}