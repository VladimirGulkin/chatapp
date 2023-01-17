using MessengerLibrary.Contracts;
using SimpleQueue;
using SimpleQueue.Contracts;

namespace MessengerLibrary.Implementation;

public class MessageBus : IMessageBus<IChatMessage>
{
    private List<ISubscriber<IChatMessage>> _subscribers;

    public MessageBus()
    {
        _subscribers = new List<ISubscriber<IChatMessage>>();
    }
    
    public void Subscribe(ISubscriber<IChatMessage> subscriber)
    {
        _subscribers.Add(subscriber);
    }

    public void PublishToSubscribers(IBusMessage<IChatMessage> message)
    {
        var sender = message.Message.Sender;
        foreach (var subscriber in _subscribers)
        {
            if(subscriber.Id != sender.Id)
                subscriber.OnMessageReceived(message.Message);
        }
    }

    public void Send(IBusMessage<IChatMessage> message, ISubscriber<IChatMessage> author)
    {
        if (MessageReceived is null)
        {
            throw new NullReferenceException($"No one is subscribed to event {nameof(MessageReceived)}");
        }

        MessageReceived?.Invoke(this, new MessageReceivedEventArgs<IChatMessage> { Message = message });
    }

    public event EventHandler<MessageReceivedEventArgs<IChatMessage>>? MessageReceived;
}