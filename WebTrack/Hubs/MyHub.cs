using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Dynamic;

namespace WebTrack.Hubs
{
    public class MyHub : Hub
    {
        private readonly ILogger<MyHub> _logger;

        public MyHub(ILogger<MyHub> logger)
        {
            _logger = logger;
        }

        public async Task ReceiveFromJs(string message)
        {
            //_logger.LogInformation($"Received from JS: {message}");
            //object messageObj = JsonConvert.DeserializeObject(message, typeof(ExpandoObject));

            await Clients.All.SendAsync("ReceiveFromServer", message);
        }
    }
}
