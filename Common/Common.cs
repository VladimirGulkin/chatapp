namespace Common
{
    public interface IUser
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }

    public enum MessageType
    {
        Text,
        Image
    }

    public interface IMessage
    {
        MessageType Type { get; set; }
        IUser FromUser { get; set; }
        byte[] Payload { get; set; }
    }

    public interface IChatConfig
    {
        string Address { get; set; }
    }
}