
using Common;

namespace Transport
{
    public enum ConnectionState
    {
        Connected,
        Connecting,
        Disconnected,
    }
    
    public interface ITransport
    {
        void Connect(IChatConfig cfg);
        void Disconnect();
    
        void Send(IMessage msg);
        IObserver<IMessage> Received { get; }
        IObserver<ConnectionState> ConnectionChanged { get; }
    }
}