using Microsoft.EntityFrameworkCore;
using SampleChatbotApi.Storage;
using SampleChatbotApi.Storage.Model;

namespace SampleChatbotApiTests;

public abstract class TestBase : IDisposable
{
    protected readonly MessagesContext MessagesContext;

    protected TestBase()
    {
        var builder = new DbContextOptionsBuilder<MessagesContext>();
        builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

        var dbContextOptions = builder.Options;
        MessagesContext = new MessagesContext(dbContextOptions);
        
        SeedData();
    }

    private void SeedData()
    {
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            UserName = "Test",
            Messages = new List<Message>()
        };
        
        MessagesContext.Conversations.Add(conversation);
            
        var message = new Message
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.Now,
            Kind = MessageKind.Chatbot,
            Text = "Hello World",
            Conversation = conversation
        };
        
        MessagesContext.Messages.Add(message);
        
        MessagesContext.SaveChanges();
    }

    public void Dispose()
    {
        MessagesContext.Messages.RemoveRange(MessagesContext.Messages);
        MessagesContext.Conversations.RemoveRange(MessagesContext.Conversations);
        MessagesContext.Dispose();
    }
}