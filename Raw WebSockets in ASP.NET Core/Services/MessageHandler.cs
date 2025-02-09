using WebSocketsServer.Messages;

namespace WebSocketsServer.Services
{
    public interface IMessagesHandler
    {
        void HandleMessage(WsMessage message, ClientConnection connection);
    }

    public class MessagesHandler : IMessagesHandler
    {
        public void HandleMessage(WsMessage message, ClientConnection connection)
        {
            switch (message.Type)
            {
                case WsMessageType.EchoRequest:
                    EchoRequest echoRequest = (EchoRequest)message;
                    EchoResponse response = new EchoResponse() { Content = echoRequest.Content };
                    connection.PushMessage(response);
                    break;
                default:
                    //unknown message from client, drop the messsage (maybe log it in the future)
                    break;
            }
        }
    }
}
