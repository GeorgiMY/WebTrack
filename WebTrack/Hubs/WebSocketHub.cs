using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using WebTrack.Data;
using WebTrack.Data.Entities;

namespace WebTrack.Hubs
{
    public class WebSocketHub : Hub
    {
        private readonly ILogger<WebSocketHub> _logger;

        public WebSocketHub(ILogger<WebSocketHub> logger)
        {
            _logger = logger;
        }

        public async Task ReceiveFromJs(string message)
        {
            string secretId = "";
            if (message.Contains("SecretId"))
            {
                secretId = message.Split("=")[1];
                await Clients.All.SendAsync($"ReceiveFromServer-{secretId}", message);
            }

            // For debugging
            //_logger.LogInformation($"Received from JS: {message}");
        }
    }
}
