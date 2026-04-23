using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using WebTrack.Core.Contracts;
using WebTrack.Core.Services;

namespace WebTrack.Hubs
{
    public class WebSocketHub : Hub
    {
        private readonly ILogger<WebSocketHub> _logger;
        private readonly IVisitorsService _visitorsService;
        private readonly ITrackedEventsService _trackedEventsService;

        public WebSocketHub(ILogger<WebSocketHub> logger, IVisitorsService visitorsService, ITrackedEventsService trackedEventsService)
        {
            _logger = logger;
            _visitorsService = visitorsService;
            _trackedEventsService = trackedEventsService;
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

            try
            {
                using var document = JsonDocument.Parse(message);
                var root = document.RootElement;

                if (root.TryGetProperty("type", out var eventTypeElement) && root.TryGetProperty("visitorId", out var visitorIdElement))
                {
                    string? eventType = eventTypeElement.GetString();
                    string? visitorId = visitorIdElement.GetString();

                    // Only log meaningful interactions to SQL!
                    //if (eventType != "MOUSE_MOVE" && eventType != "DOM_UPDATE")
                    //{
                        // Fire and forget the database save
                        await _trackedEventsService.LogEventAsync(visitorId, eventType);
                    //}
                }
            }
            catch (JsonException)
            {
                // Ignore parsing errors from weird payloads, just keep the hub alive
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
