using GridOperationCodeSender;
using GridOperationCodeSender.models.configuration;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(ConfigureAppConfig)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
        var serviceConfig = context.Configuration.Get<WorkerConfig>();
        services.AddSingleton<WorkerConfig>(serviceConfig);
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