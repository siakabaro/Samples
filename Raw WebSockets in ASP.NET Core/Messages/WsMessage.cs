using Newtonsoft.Json;

namespace WebSocketsServer.Messages
{
    public enum WsMessageType
    {
        EchoRequest,
        EchoResponse,
        ServerResourcesEvent,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class WsMessageTypeAttribute : Attribute
    {
        public WsMessageType Type { get; set; }
        public WsMessageTypeAttribute(WsMessageType type)
        {
            Type = type;
        }
    }

    public abstract class WsMessage
    {

        [JsonIgnore]
        public WsMessageType Type
        {
            get
            {
                return this.GetType().GetCustomAttributes(typeof(WsMessageTypeAttribute), false).OfType<WsMessageTypeAttribute>().First().Type;
            }
        }

        public MessageEnvelope BuildEnvelope()
        {
            return new MessageEnvelope()
            {
                MessageType = this.Type,
                JsonPayload = JsonConvert.SerializeObject(this)
            };
        }
    }
}
