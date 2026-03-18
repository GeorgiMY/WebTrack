using Microsoft.AspNetCore.SignalR;

namespace WebTrack.Hubs
{
    public class MyHub : Hub
    {
        private readonly ILogger<MyHub> _logger;

        public MyHub(ILogger<MyHub> logger)
        {
            _logger = logger;
        }

        public Task ReceiveFromJs(string message)
        {
            _logger.LogInformation($"Received from JS: {message}");
            return Task.CompletedTask;
        }
    }
}
