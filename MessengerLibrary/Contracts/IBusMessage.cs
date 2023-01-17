namespace MessengerLibrary.Contracts;

public interface IBusMessage<out T>
{
    T Message { get; }
}