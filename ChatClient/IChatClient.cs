using Common;
using Transport;

namespace ChatClient
{
    public interface IChatClient
    {
        void Start(ITransport transport, IUser user);
        void Stop();

        void Send(IMessage msg);
        IObserver<IMessage> Received { get; }
    }
}
