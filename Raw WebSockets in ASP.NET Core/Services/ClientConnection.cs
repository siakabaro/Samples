using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using WebSocketsServer.Messages;

namespace WebSocketsServer.Services
{
    public class ClientConnection
    {
        private WebSocket _webSocket;
        private ConcurrentQueue<WsMessage> _wsMessagesToPush = new ConcurrentQueue<WsMessage>();
        private Task? _pushMessagesTask;
        private IMessagesHandler _messageHandler;
        private ILogger<ConnectionManager> _logger;

        public ClientConnection(WebSocket webSocket, IMessagesHandler handler, ILogger<ConnectionManager> logger)
        {
            _webSocket = webSocket;
            _messageHandler = handler;
            _logger = logger;
            ClientId = Guid.NewGuid().ToString();
        }

        public string ClientId { get; private set; }
        public bool IsConnected { get { return _webSocket.State == WebSocketState.Open; } }

        /// <summary>
        /// Enqueue the message to the push queue.
        /// </summary>
        public void PushMessage(WsMessage message)
        {
            _wsMessagesToPush.Enqueue(message);
        }

        public async Task<MessageEnvelope?> GetMessageEnvelopeAsync(CancellationToken cancellationToken)
        {
            WebSocketReceiveResult receiveResult;
            var tempBuffer = new byte[1024 * 4];
            byte[]? completeMessage;
            using (MemoryStream messageStream = new MemoryStream())
            {
                do
                {
                    _logger.LogInformation("Calling ReceiveAsync");
                    receiveResult = await _webSocket.ReceiveAsync(new ArraySegment<byte>(tempBuffer), cancellationToken);

                    _logger.LogInformation($"After ReceiveAsync, count: {receiveResult.Count}, endmessage: {receiveResult.EndOfMessage} close status:{receiveResult.CloseStatus.HasValue} ");
                    if (receiveResult.CloseStatus.HasValue)
                    {
                        // If we received a close message from the remote endpoint, we need to send a close message and stop reading the message.
                        _logger.LogInformation($"Received a close message from client:{this.ClientId}");
                        await _webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, cancellationToken);
                        return null;
                    }
                    // Append to received bytes if the websocket received a non-empty message.
                    if (receiveResult.Count > 0)
                    {
                        messageStream.Write(tempBuffer, 0, receiveResult.Count);
                    }
                }
                // Stop processing if we reach the end of a message 
                while (!receiveResult.EndOfMessage && !cancellationToken.IsCancellationRequested);

                completeMessage = messageStream.ToArray();
            }

            return BuildMessageEnvelope(completeMessage);
        }

        private MessageEnvelope? BuildMessageEnvelope(byte[]? completeMessage)
        {
            try
            {
                if (completeMessage == null || completeMessage.Length == 0)
                {
                    return null;
                }
                else
                {
                    return JsonConvert.DeserializeObject<MessageEnvelope>(Encoding.UTF8.GetString(completeMessage));
                }
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                // Log the serialization error and return null.
                _logger.LogError("Received an unformatted message. Exception: " + ex.ToString());
                return null;
            }
        }

        public async Task ProcessWebSocketMessagesAsync(CancellationToken cancellationToken)
        {
            // Start thread to push pending messages to the client.
            _pushMessagesTask = Task.Run(async () => await PushPendingMessagesAsync(cancellationToken));

            // Receive and process incoming messages from the client.
            try
            {
                while (!cancellationToken.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
                {
                    var message = (await GetMessageEnvelopeAsync(cancellationToken))?.ExtractMessage();
                    if (message != null)
                    {
                        _messageHandler.HandleMessage(message, this);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Connection lost, exit
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while receiving WebSocket messages: " + ex.ToString());
            }

            await _pushMessagesTask;
        }

        public async Task PushPendingMessagesAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_wsMessagesToPush.TryDequeue(out var message))
                {
                    var envelope = message.BuildEnvelope();
                    var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(envelope));
                    _logger.LogInformation("Pushing data");
                    await _webSocket.SendAsync(messageBytes, WebSocketMessageType.Text, true, cancellationToken);
                }
                if (_wsMessagesToPush.Count == 0)
                {
                    // If nothing in the queue, wait 500ms before continuing the loop.
                    await Task.Delay(500);
                }
            }
        }
    }
}
