namespace WebSocketsServer.Messages
{
    [WsMessageType(WsMessageType.ServerResourcesEvent)]
    public class ServerResourcesEvent : WsMessage
    {
        public double CpuUsedPercentage { get; set; }
        public ulong MemoryUsedInBytes { get; set; }
        public double MemoryUsedPercentage { get; set; }
        public double MaximumCpuUnits { get; set; }
        public ulong MaximumMemoryInBytes { get; set; }
    }
}
