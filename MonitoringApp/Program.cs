using InfluxDB.Client;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR.Client;
using MonitoringApp.Components;
using MonitoringApp.models;
using MonitoringApp.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appConfig = builder.Configuration.Get<AppConfiguration>();
builder.Services.AddSingleton<AppConfiguration>(appConfig);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSignalR()
    .AddHubOptions<RealTimeHub>(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddScoped<HttpClient>(sp => new HttpClient
{
    BaseAddress = new Uri(appConfig.ScheduleManagementApiUri)
});

builder.Services.AddScoped<HubConnection>((serviceProvider) =>
{
    var hubConnection = new HubConnectionBuilder()
        .WithUrl("http://localhost:5256/realtimehub")
        .WithAutomaticReconnect()
        .Build();

    return hubConnection;
});

builder.Services.AddSingleton(sp => new InfluxDBClient(appConfig.InfluxDB.Uri, appConfig.InfluxDB.Token));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map the SignalR hub
app.MapHub<RealTimeHub>("/realtimehub");

app.Run();

app.UseRouting();