namespace MessengerLibrary.Contracts;

public interface IClient 
{
    void SendMessage(IChatMessage message);
    Guid ConnectedUserId { get; }
}