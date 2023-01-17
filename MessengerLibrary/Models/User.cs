using System.Text.RegularExpressions;

namespace MessengerLibrary.Models;

public class User
{
    internal DateTime LastActiveDateTime;
    public Guid Id { get; }

    public User(string name)
    {
        ValidateName(name);
        Name = name;
        LastActiveDateTime = DateTime.UtcNow;
        Id = Guid.NewGuid();
    }

    public string Name { get; }

    private void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(name, "User name cannot be null or empty");
            
        if (name.Length > 50)
            throw new ArgumentOutOfRangeException(name, "Max number of characters is 50");

        var pattern = new Regex("^[a-zA-Z0-9 ]*$");
        if (!pattern.IsMatch(name))
            throw new ArgumentOutOfRangeException(name, "There must be no special characters in the name");
    }
}