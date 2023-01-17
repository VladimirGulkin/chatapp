namespace SimpleQueue.Contracts;

public interface ISubscriber<in T>
{
    Guid Id { get; }
    void OnMessageReceived(T message);
}