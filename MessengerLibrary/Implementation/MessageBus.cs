using MessengerLibrary.Contracts;

namespace MessengerLibrary.Implementation;

public class MessageBus : IMessageBus
{
    private List<ISubscriber> _subscribers;

    public MessageBus()
    {
        _subscribers = new List<ISubscriber>();
    }
    
    public void Subscribe(ISubscriber subscriber)
    {
        _subscribers.Add(subscriber);
    }

   
    public void Publish<IChatMessage>(IBusMessage<IChatMessage> message, ISubscriber sender)
    {
        foreach (var subscriber in _subscribers)
        {
            if(!subscriber.Equals(sender))
                subscriber.OnMessageReceived((Contracts.IChatMessage)message.Message);
        }
    }
}