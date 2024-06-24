using Microsoft.AspNetCore.SignalR;

namespace MonitoringApp.SignalR;

public class RealTimeHub : Hub
{
    public async Task SendMessage(string value)
    {
        Console.WriteLine("---Signal-R-SendMessage-forwarding..." + value);
        await Clients.All.SendAsync("ReceiveMessage", value);
    }
}