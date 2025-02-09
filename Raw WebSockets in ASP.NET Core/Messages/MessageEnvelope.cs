using Newtonsoft.Json;
namespace WebSocketsServer.Messages
{
    public class MessageEnvelope
    {
        public WsMessageType MessageType { get; set; }
        public string? JsonPayload { get; set; }

        public WsMessage? ExtractMessage()
        {
            if (JsonPayload == null)
                throw new InvalidOperationException("Cannot deserialize a null json payload");

            switch (MessageType)
            {
                case WsMessageType.EchoRequest:
                    return JsonConvert.DeserializeObject<EchoRequest>(JsonPayload);
                case WsMessageType.EchoResponse:
                    return JsonConvert.DeserializeObject<EchoResponse>(JsonPayload);
                case WsMessageType.ServerResourcesEvent:
                    return JsonConvert.DeserializeObject<ServerResourcesEvent>(JsonPayload);
                default:
                    throw new NotSupportedException($"Message type is not supported: {MessageType}");

            }
        }
    }
}
