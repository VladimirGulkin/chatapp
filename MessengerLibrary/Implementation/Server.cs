using MessengerLibrary.Contracts;
using MessengerLibrary.Models;

namespace MessengerLibrary.Implementation;

public class Server : IServer
{
    private IMessageBus _messageBus;

    public Server(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public void RegisterClient(IClient client)
    {
        if (client is null)
            throw new ArgumentNullException(nameof(client));

        if (client.ConnectedUserId == Guid.Empty)
            throw new ArgumentException("Client with empty Id cannot be connected", nameof(client.ConnectedUserId));
        
        _messageBus.Subscribe((ISubscriber)client);
    }

    public void SendMessageToClients(Message message, IClient sender)
    {
        if (sender.ConnectedUserId == Guid.Empty)
            throw new ArgumentException("Client Id could not be empty");
        
        _messageBus.Publish(new BusMessage(message), (ISubscriber)sender);
    }
}