using Confluent.Kafka;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
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

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(configuration.Kafka.Topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var cr = consumer.Consume(stoppingToken);
                logger.LogInformation("Consumed message '{Message}' from '{Key}' at: '{TopicPartitionOffset}'.", cr.Message.Value, cr.Message.Key,
                    cr.TopicPartitionOffset);

                await _hubConnection.SendAsync("SendMessage", cr.Message.Key, cr.Message.Value, cancellationToken: stoppingToken);
               
                using var client = new InfluxDBClient("http://localhost:8086", configuration.InfluxDB.Token);
                var point = PointData
                    .Measurement("power_consumption")
                    .Tag("consumer", cr.Message.Key)
                    .Field("watt", decimal.Parse(cr.Message.Value))
                    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

                using var writeApi = client.GetWriteApi();
                writeApi.WritePoint(point, configuration.InfluxDB.Bucket, configuration.InfluxDB.Org);
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