using MessengerLibrary.Contracts;
using MessengerLibrary.Implementation;
using MessengerLibrary.Models;

namespace MessengerLibrary;

public class Client : IClient, ISubscriber
{
    private IMessageBus _messageBus;
    private Action<IChatMessage>? _incomingMessageHandler;
    public Guid ConnectedUserId { get; private set; } = Guid.Empty;

    public static Client CreateClient(IMessageBus messageBus, Action<IChatMessage>? incomingMessageHandler)
    {
        return new(messageBus, incomingMessageHandler);
    }

    public void ConnectUserToClient(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user),"User could not be null");
        
        ConnectedUserId = user.Id;
    }

    private Client(IMessageBus messageBus, Action<IChatMessage>? incomingMessageHandler)
    {
        _messageBus = messageBus;
        _incomingMessageHandler = incomingMessageHandler;
    }

    public void SendMessage(IChatMessage message)
    {
        if (IsUsernameExpired(message.Sender))
        {
            ConnectedUserId = Guid.Empty;
            throw new TimeoutException(
                $"User inactivity limit expired; Last activity time {message.Sender.LastActiveDateTime}, current time {DateTime.Now}");
        }

        message.Sender.LastActiveDateTime = DateTime.UtcNow;
        _messageBus.Publish<IChatMessage>(new BusMessage(message), this);
    }

    private bool IsUsernameExpired(User user)
    {
        return user.LastActiveDateTime.Add(TimeSpan.FromMinutes(10)) <= DateTime.UtcNow;
    }

    public void OnMessageReceived(IChatMessage message)
    {
        if(_incomingMessageHandler != null)
            _incomingMessageHandler(message);
    }

    public override bool Equals(object? obj)
    {
        if(obj is Client client)
            return this.ConnectedUserId == client.ConnectedUserId;
        return false;
    }
}