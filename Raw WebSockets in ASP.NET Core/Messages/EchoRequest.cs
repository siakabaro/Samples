namespace WebSocketsServer.Messages
{
    [WsMessageType(WsMessageType.EchoRequest)]
    public class EchoRequest : WsMessage
    {
        public string? Content { get; set; }
    }
}
