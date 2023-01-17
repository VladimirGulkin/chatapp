namespace SimpleQueue.Contracts;

public interface IMessageBus<T> where T: class
{
    void Subscribe(ISubscriber<T> subscriber);
    void PublishToSubscribers(IBusMessage<T> message);
    void Send(IBusMessage<T> message, ISubscriber<T> author);
    event EventHandler<MessageReceivedEventArgs<T>> MessageReceived;
}