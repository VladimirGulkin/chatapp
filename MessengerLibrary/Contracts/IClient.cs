namespace MessengerLibrary.Contracts;

public interface IClient
{
    Guid Id { get; }
    void SendMessage(IChatMessage message);
    void ConnectToServer();
}