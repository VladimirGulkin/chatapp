namespace MessengerLibrary.Contracts;

public interface IMessageBus
{
    void Subscribe(ISubscriber subscriber);
    void Publish<T>(IBusMessage<T> message, ISubscriber sender);
}