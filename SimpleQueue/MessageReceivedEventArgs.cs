using SimpleQueue.Contracts;

namespace SimpleQueue;

public class MessageReceivedEventArgs<T> : EventArgs 
    where T : class
{
    public IBusMessage<T> Message;
}