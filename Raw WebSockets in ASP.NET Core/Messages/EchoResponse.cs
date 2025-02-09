namespace WebSocketsServer.Messages
{
    [WsMessageType(WsMessageType.EchoResponse)]
    public class EchoResponse : WsMessage
    {
        public string? Content { get; set; }
    }
}
