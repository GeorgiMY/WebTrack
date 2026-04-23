using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebTrack.Tests.Hubs;

public class MyHubTests
{
    //[Test]
    //public async Task ReceiveFromJs_WithoutSecretId_LogsWarning()
    //{
    //    Mock<ILogger<WebSocketHub>> loggerMock = new();
    //    Mock<IHubCallerClients> clientsMock = new();
    //    WebSocketHub hub = new(loggerMock.Object)
    //    {
    //        Clients = clientsMock.Object,
    //        Context = CreateHubContext()
    //    };

    //    await hub.ReceiveFromJs("hello");

    //    loggerMock.Verify(
    //        logger => logger.Log(
    //            LogLevel.Warning,
    //            It.IsAny<EventId>(),
    //            It.Is<It.IsAnyType>((value, _) => value.ToString()!.Contains("without a secret_id")),
    //            It.IsAny<Exception>(),
    //            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
    //        Times.Once);
    //}

    private static HubCallerContext CreateHubContext()
    {
        return new TestHubCallerContext(new FeatureCollection());
    }

    private sealed class TestHubCallerContext(IFeatureCollection features) : HubCallerContext
    {
        public override string ConnectionId => "test-connection";
        public override string UserIdentifier => "test-user";
        public override ClaimsPrincipal? User => null;
        public override IDictionary<object, object?> Items { get; } = new Dictionary<object, object?>();
        public override IFeatureCollection Features { get; } = features;
        public override CancellationToken ConnectionAborted => CancellationToken.None;
        public override void Abort() { }
    }

}
