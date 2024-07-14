using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RealTimeDataConsumer.models.configuration;
using ScheduleManagementApi.models;

namespace ScheduleManagementApi;

public class ConsumerConfigurationRepository
{
    private readonly IMongoCollection<ConsumerConfiguration> _consumersCollection;

    public ConsumerConfigurationRepository(
        IOptions<MongoDBSettings> consumerStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            consumerStoreDatabaseSettings.Value.ConnectionURI);

        var mongoDatabase = mongoClient.GetDatabase(
            consumerStoreDatabaseSettings.Value.DatabaseName);

        _consumersCollection = mongoDatabase.GetCollection<ConsumerConfiguration>(
            consumerStoreDatabaseSettings.Value.CollectionName);
    }

    public async Task<List<ConsumerConfiguration>> GetAsync() =>
        await _consumersCollection.Find(_ => true).ToListAsync();

    public async Task<ConsumerConfiguration?> GetAsync(string id) =>
        await _consumersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ConsumerConfiguration newConsumer) =>
        await _consumersCollection.InsertOneAsync(newConsumer);
    
    public async Task CreateWallboxAsync(WallboxConfiguration newConsumer) =>
        await _consumersCollection.InsertOneAsync(newConsumer);
    
    public async Task CreateHeatpumpAsync(HeatpumpConfiguration newConsumer) =>
        await _consumersCollection.InsertOneAsync(newConsumer);

    public async Task UpdateAsync(string id, ConsumerConfiguration updatedConsumer) =>
        await _consumersCollection.ReplaceOneAsync(x => x.Id == id, updatedConsumer);

    public async Task RemoveAsync(string id) =>
        await _consumersCollection.DeleteOneAsync(x => x.Id == id);
}