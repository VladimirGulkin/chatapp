namespace MessengerLibrary.Contracts;

public interface ISubscriber
{
    void OnMessageReceived(IChatMessage message);
}