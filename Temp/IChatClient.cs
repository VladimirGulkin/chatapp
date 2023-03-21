
namespace ConsoleTmp;

public enum ConnectionState
{
    Connected,
    Connecting,
    Disconnected,
}

public interface IConnection
{
    string ServiceUrl { get; set; }
    ConnectionState ConnectionState { get; set; }
}

// public enum UserType
// {
//     Admin,
//     JustUser
// }

public interface IUser
{
    Guid Id { get; set; }
    string Name { get; set; }
}

public enum MessageType
{
    Text,
    Image
}

public interface IMessage
{
    MessageType Type { get; set; }
    IUser User { get; set; }
    byte[] Payload { get; set; }
}

public interface IChatClient
{
    //IChatTransport
    //IUser
    
    //todo move to .ctor
    void Initialize(IChatTransport transport); 
    IConnection Start(IUser user); 
    IConnection Stop();

    void Send(IMessage msg);
    void Send(string text);
    void Send(byte[] image);
    IObserver<IMessage> Received { get; }
}

public interface ITransportConfig
{
    string ServiceUrl { get; set; }
}

public interface IChatTransport
{
    //todo move to .ctor
    void Initialize(ITransportConfig cfg); 
    IConnection Start(IUser user);
    IConnection Stop(IUser user);
    void Send(IMessage msg);
    IObserver<IMessage> Received { get; }
    IObserver<IConnection> ConnectionChanged { get; }
}