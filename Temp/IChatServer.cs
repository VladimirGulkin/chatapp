namespace ConsoleTmp;

public interface IChatServerConfig
{
    string Address { get; set; }
}

public interface IChatServer
{
    void Start(IChatServerConfig cgf);
    void Stop();
}