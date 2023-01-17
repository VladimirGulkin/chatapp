using MessengerLibrary.Contracts;

namespace MessengerLibrary.Implementation;

public class BusMessage : IBusMessage<IChatMessage>
{
    public IChatMessage Message { get; }

    public BusMessage(IChatMessage message)
    {
        Message = message;
    }
}