using Microsoft.AspNetCore.SignalR;
using BackgroundEmailService.MVC.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackgroundEmailService.MVC.Hubs
{
    public class EmailHub : Hub
    {
        public async Task SendMessage(string sender, EmailViewModel message)
        {
            await Clients.All.SendAsync("emailObserver", sender, JsonSerializer.Serialize(message));
        }
    }
}
