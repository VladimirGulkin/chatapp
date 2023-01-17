using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MessengerLibrary;
using MessengerLibrary.Contracts;
using NUnit.Framework;
using MessengerLibrary.Implementation;
using MessengerLibrary.Models;
using SimpleQueue.Contracts;

namespace MessengerTests;

public class Tests
{
    private IMessageBus<IChatMessage> _messageBus;
    private IServer _server;

    [SetUp]
    public void Setup()
    {
        _messageBus = new MessageBus();
        _server = new Server(_messageBus);
        _server.InitializeConnection();
    }

    [Test]
    public void NotGettingOwnMessagesTest()
    {
        //Check count of messages
        var userA = new User("UserA");
        var userB = new User("UserB");
        var clientAHistory = new List<IChatMessage>();
        var clientBHistory = new List<IChatMessage>();
        var clientA = Client.CreateClient(_messageBus, clientAHistory.Add, userA);
        var clientB = Client.CreateClient(_messageBus, clientBHistory.Add, userB);
        clientA.ConnectToServer();
        clientB.ConnectToServer();
        var userAMessage = new Message(userA, $"Hello, I'm {userA.Name}");
        var userBMessage = new Message(userB, $"Hello, I'm {userB.Name}");

        clientA.SendMessage(userAMessage);
        clientB.SendMessage(userBMessage);
        
        Assert.AreEqual(1, clientAHistory.Count, $"Received messages count mismatch: expected 1 message, but was {clientAHistory.Count}");
        Assert.AreEqual(1, clientAHistory.Count, $"Received messages count mismatch: expected 1 message, but was {clientBHistory.Count}");
        
        Assert.That(string.Equals(clientAHistory.FirstOrDefault(), userBMessage));
        Assert.That(string.Equals(clientBHistory.FirstOrDefault(), userAMessage));
    }

    [Test]
    public void UserExpirationTest()
    {
        var user = new User("user");
        var client = Client.CreateClient(_messageBus, null, user);
        client.ConnectToServer();
       

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
        var firstClient = Client.CreateClient(_messageBus, null, user);
        firstClient.ConnectToServer();
      
        var secondClient = Client.CreateClient(_messageBus, messageCache.Add, user2);
        secondClient.ConnectToServer();
        
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
        
        Assert.Throws<ArgumentNullException>(() => Client.CreateClient(_messageBus, null, user));
    }
    
    [Test]
    public void CantCreateClientWithoutMessageBus()
    {
        var user = new User("Test user");
        Assert.Throws<ArgumentNullException>(() => Client.CreateClient(null, null, user));
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