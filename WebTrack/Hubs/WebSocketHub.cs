using Microsoft.AspNetCore.SignalR;
using WebTrack.Core.Contracts;

namespace WebTrack.Hubs
{
    public class WebSocketHub : Hub
    {
        private readonly ILogger<WebSocketHub> _logger;
        private readonly IVisitorsService _visitorsService;

        public WebSocketHub(ILogger<WebSocketHub> logger, IVisitorsService visitorsService)
        {
            _logger = logger;
            _visitorsService = visitorsService;
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

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //await _visitorsService.EndSessionAsync(Context.ConnectionId);

            //await base.OnDisconnectedAsync(exception);
        }
    }
}
