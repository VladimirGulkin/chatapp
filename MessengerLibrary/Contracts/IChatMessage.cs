using MessengerLibrary.Models;

namespace MessengerLibrary.Contracts;

public interface IChatMessage
{
    User Sender { get; }
    string Text { get; }
}