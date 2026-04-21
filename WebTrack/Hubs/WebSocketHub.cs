using Microsoft.AspNetCore.SignalR;

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
            HttpContext? httpContext = Context.GetHttpContext();
            string? secretId = httpContext?.Request.Query["secret_id"].ToString();

            if (!string.IsNullOrEmpty(secretId))
            {
                await Clients.All.SendAsync($"ReceiveFromServer-{secretId}", message);
            }
            else
            {
                _logger.LogWarning("Received message from a connection without a secret_id.");
            }

            // For debugging
            //_logger.LogInformation($"Received from JS: {message}");
        }
    }
}
