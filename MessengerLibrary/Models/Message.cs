using MessengerLibrary.Contracts;

namespace MessengerLibrary.Models;

public class Message : IChatMessage
{
    public User Sender { get; }
    public string Text { get; }

    public Message(User sender, string text)
    {
        Sender = sender;
        Text = text;
    }

    public override string ToString()
    {
        return $"{Sender.Name}: {Text}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Message other)
            return this.Text == other.Text;
        
        return false;
    }
}