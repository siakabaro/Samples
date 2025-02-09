using Microsoft.AspNetCore.Mvc;
using WebSocketsServer.Services;

namespace WebSocketsServer.Controllers
{
    [ApiController]
    public class ResourcesMonitoringController : ControllerBase
    {
        private readonly IConnectionManager _connectionManager;

        public ResourcesMonitoringController(ILogger<ResourcesMonitoringController> logger, IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        [Route("resources/ws")]
        [HttpGet]
        public async Task Get(CancellationToken cancellationToken)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    await _connectionManager.HandleClientAsync(webSocket, cancellationToken);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

    }
}
