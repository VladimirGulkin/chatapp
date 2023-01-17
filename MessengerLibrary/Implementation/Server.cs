using MessengerLibrary.Contracts;
using SimpleQueue;
using SimpleQueue.Contracts;

namespace MessengerLibrary.Implementation;

public class Server : IServer
{
    private IMessageBus<IChatMessage> _messageBus;

    public Server(IMessageBus<IChatMessage> messageBus)
    {
        _messageBus = messageBus;
    }

    public void InitializeConnection()
    {
        _messageBus.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs<IChatMessage> e)
    {
        if (e.Message.Message.Sender.Id == Guid.Empty)
            throw new ArgumentException("Client Id could not be empty");

        _messageBus.PublishToSubscribers(e.Message);
    }
}