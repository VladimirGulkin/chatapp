using MessengerLibrary.Contracts;
using MessengerLibrary.Implementation;
using MessengerLibrary.Models;
using SimpleQueue.Contracts;

namespace MessengerLibrary;

public class Client : IClient, ISubscriber<IChatMessage>
{
    private IMessageBus<IChatMessage> _messageBus;
    private Action<IChatMessage>? _incomingMessageHandler;
    public Guid Id { get; private set; } = Guid.Empty;

    public static Client CreateClient(IMessageBus<IChatMessage> messageBus, Action<IChatMessage>? incomingMessageHandler, User user)
    {
        return new(messageBus, incomingMessageHandler, user);
    }

    private Client(IMessageBus<IChatMessage> messageBus, Action<IChatMessage>? incomingMessageHandler, User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user),"User cannot be null");

        Id = user.Id;
        _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus),"User cannot be null");
        _incomingMessageHandler = incomingMessageHandler;
    }

    public void SendMessage(IChatMessage message)
    {
        if (IsUsernameExpired(message.Sender))
        {
            Id = Guid.Empty;
            throw new TimeoutException(
                $"User inactivity limit expired; Last activity time {message.Sender.LastActiveDateTime}, current time {DateTime.Now}");
        }

        message.Sender.LastActiveDateTime = DateTime.UtcNow;
        _messageBus.Send(new BusMessage(message), this);
    }

    public void ConnectToServer()
    {
        _messageBus.Subscribe(this);
    }

    private bool IsUsernameExpired(User user)
    {
        return user.LastActiveDateTime.Add(TimeSpan.FromMinutes(10)) <= DateTime.UtcNow;
    }

    public void OnMessageReceived(IChatMessage message)
    {
        _incomingMessageHandler?.Invoke(message);
    }

    public override bool Equals(object? obj)
    {
        if(obj is Client client)
            return this.Id == client.Id;
        return false;
    }
}