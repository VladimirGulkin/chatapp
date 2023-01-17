namespace SimpleQueue.Contracts;

public interface IBusMessage<out T> where T: class
{
    T Message { get; }
}