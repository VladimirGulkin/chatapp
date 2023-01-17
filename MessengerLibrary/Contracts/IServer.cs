using MessengerLibrary.Models;

namespace MessengerLibrary.Contracts;

public interface IServer
{
    void RegisterClient(IClient client);
    void SendMessageToClients(Message message, IClient sender);
}