using Common;
using Transport;

namespace ChatServer
{
    public interface IChatServer
    {
        void Start(ITransport transport);
        void Stop();
    }
}
