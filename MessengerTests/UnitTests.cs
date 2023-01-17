using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MessengerLibrary;
using MessengerLibrary.Contracts;
using NUnit.Framework;
using MessengerLibrary.Implementation;
using MessengerLibrary.Models;

namespace MessengerTests;

public class Tests
{
    private IMessageBus _messageBus;
    private IServer _server;

    [SetUp]
    public void Setup()
    {
        _messageBus = new MessageBus();
        _server = new Server(_messageBus);
    }

    [Test]
    public void NotGettingOwnMessagesTest()
    {
        var userA = new User("UserA");
        var userB = new User("UserB");
        var clientAHistory = new List<IChatMessage>();
        var clientBHistory = new List<IChatMessage>();
        var clientA = Client.CreateClient(_messageBus, clientAHistory.Add);
        var clientB = Client.CreateClient(_messageBus, clientBHistory.Add);
        clientA.ConnectUserToClient(userA);
        clientB.ConnectUserToClient(userB);
        var userAMessage = new Message(userA, $"Hello, I'm {userA.Name}");
        var userBMessage = new Message(userB, $"Hello, I'm {userB.Name}");
        _server.RegisterClient(clientA);
        _server.RegisterClient(clientB);

        clientA.SendMessage(userAMessage);
        clientB.SendMessage(userBMessage);

        var clientAReceivedMessage = clientAHistory.FirstOrDefault();
        var clientBReceivedMessage = clientBHistory.FirstOrDefault();
        
        Assert.AreEqual(1, clientAHistory.Count, $"Messages count mismatch: expected 1, but got {clientAHistory.Count}");
        Assert.AreEqual(1, clientBHistory.Count, $"Messages count mismatch: expected 1, but got {clientBHistory.Count}");

        Assert.That(string.Equals(clientAReceivedMessage, userBMessage));
        Assert.That(string.Equals(clientBReceivedMessage, userAMessage));
    }

    [Test]
    public void UserExpirationTest()
    {
        var user = new User("user");
        var client = Client.CreateClient(_messageBus, null);
        client.ConnectUserToClient(user);
        _server.RegisterClient(client);

        var typeOfUser = typeof(User);
        var field = typeOfUser.GetField("LastActiveDateTime", BindingFlags.NonPublic | BindingFlags.Instance);
        var incorrectDateTime = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(11.1));
        field.SetValue(user, incorrectDateTime);

        Assert.Throws<TimeoutException>(() => client.SendMessage(new Message(user, "This should throw")),
            "When trying to send message with user older than 10 minutes, message sending should throw exception");
    }

    [Test]
    public void HandlerReceivesMessageTest()
    {
        var messageCache = new List<IChatMessage>();
        var user = new User("user");
        var user2 = new User("user2");
        var firstClient = Client.CreateClient(_messageBus, null);
        firstClient.ConnectUserToClient(user);
        _server.RegisterClient(firstClient);
        var secondClient = Client.CreateClient(_messageBus, messageCache.Add);
        secondClient.ConnectUserToClient(user2);
        _server.RegisterClient(secondClient);
        var msg1 = new Message(user, "First message");
        var msg2 = new Message(user, "Second message");
        var msg3 = new Message(user, "Third message");

        firstClient.SendMessage(msg1);
        firstClient.SendMessage(msg2);
        firstClient.SendMessage(msg3);

        var actualMessagesCount = messageCache.Count;
        var expectedMessagesCount = 3;
        Assert.That(() => actualMessagesCount == expectedMessagesCount,
            $"Message count mismatch; expected: {expectedMessagesCount}, actual: {actualMessagesCount}");
    }

    [Test]
    public void CantConnectNullUser()
    {
        User user = null;
        var client = Client.CreateClient(_messageBus, null);
        
        Assert.Throws<ArgumentNullException>(() => client.ConnectUserToClient(user));
    }
    
    [Test]
    public void CantConnectClientWithoutId()
    {
        var client = Client.CreateClient(_messageBus, null);
        Assert.Throws<ArgumentException>(() => _server.RegisterClient(client));
    }
    
    [Test]
    public void CantConnectNullClient()
    {
        Client client = null;
        Assert.Throws<ArgumentNullException>(() => _server.RegisterClient(client));
    }
    
    [Test]
    public void CreateUserWithoutName()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new User(""); 
        });
    }
}