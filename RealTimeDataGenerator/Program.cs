using RealTimeDataGenerator;
using RealTimeDataGenerator.models;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(ConfigureAppConfig)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
        var serviceConfig = context.Configuration.Get<WorkerConfig>();
        services.AddSingleton<WorkerConfig>(serviceConfig);
        services.AddSingleton<IConsumerDevicesFacade, ConsumerDevicesFacade>();
        services.AddSingleton<IRealTimeDataRepository, SimulatedRealTimeDataRepository>();
        services.AddHttpClient("apiClient", client =>
        {
            client.BaseAddress = new Uri(serviceConfig.ScheduleManagementApiUri);
            // You can set other HttpClient options here, such as timeout, headers, etc.
        });
    })
    .UseSerilog((hostingContext, loggerConfig) => loggerConfig.ReadFrom.Configuration(hostingContext.Configuration))
    .UseConsoleLifetime()
    .Build();

void ConfigureAppConfig(HostBuilderContext hostingContext, IConfigurationBuilder builder)
{
    builder.SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();

    if (!hostingContext.HostingEnvironment.IsDevelopment()) return;
}

await host.RunAsync();