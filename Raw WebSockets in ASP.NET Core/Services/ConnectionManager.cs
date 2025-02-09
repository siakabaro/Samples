using System.Net.WebSockets;
using Microsoft.Extensions.Diagnostics.ResourceMonitoring;
using WebSocketsServer.Messages;

namespace WebSocketsServer.Services
{

    public interface IConnectionManager
    {
        Task HandleClientAsync(WebSocket webSocket, CancellationToken cancellationToken);
    }

    public class ConnectionManager : IConnectionManager
    {
        private List<ClientConnection> _clientConnections = new List<ClientConnection>();
        private IMessagesHandler _messagesHandler;
        private IResourceMonitor _resourceMonitor;
        ILogger<ConnectionManager> _logger;

        public ConnectionManager(ILogger<ConnectionManager> logger, IMessagesHandler handler, IResourceMonitor resourceMonitor)
        {
            _logger = logger;
            _messagesHandler = handler;
            _resourceMonitor = resourceMonitor;
            Task.Run(DispatchResourcesEvents);
        }

        public async Task HandleClientAsync(WebSocket webSocket, CancellationToken cancellationToken)
        {
            var connection = new ClientConnection(webSocket, _messagesHandler, _logger);
            AddClientConnection(connection);
            _logger.LogInformation($"New client connected: {connection.ClientId}");
            try
            {
                await connection.ProcessWebSocketMessagesAsync(cancellationToken);
            }
            finally
            {
                _logger.LogInformation($"Client connection closed. Connected: {connection.IsConnected}");
                RemoveClientConnection(connection);
            }
        }

        private async Task DispatchResourcesEvents()
        {
            var window = TimeSpan.FromSeconds(5);
            while (true)
            {
                _logger.LogInformation($"Number of clients connected: {_clientConnections.Count}");
                if (_clientConnections.Count > 0)
                {
                    var utilization = _resourceMonitor.GetUtilization(window);
                    var resources = utilization.SystemResources;
                    ServerResourcesEvent resEvent = new ServerResourcesEvent()
                    {
                        CpuUsedPercentage = utilization.CpuUsedPercentage,
                        MaximumCpuUnits = resources.MaximumCpuUnits,
                        MaximumMemoryInBytes = resources.MaximumMemoryInBytes,
                        MemoryUsedInBytes = utilization.MemoryUsedInBytes,
                        MemoryUsedPercentage = utilization.MemoryUsedPercentage
                    };
                    Parallel.ForEach(_clientConnections, conn => conn.PushMessage(resEvent));
                }
                await Task.Delay(window);
            }
        }

        private void AddClientConnection(ClientConnection connection)
        {
            lock (_clientConnections)
            {
                _clientConnections.Add(connection);
            }
        }

        private void RemoveClientConnection(ClientConnection connection)
        {
            lock (_clientConnections)
            {
                _clientConnections.Remove(connection);
            }
        }
    }
}
