using Microsoft.EntityFrameworkCore;
using SampleChatbotApi.Storage.Model;

namespace SampleChatbotApi.Storage;

public class MessagesContext(DbContextOptions<MessagesContext> options) : DbContext(options)
{
    public DbSet<Conversation> Conversations { get; init; }
    public DbSet<Message> Messages { get; init; }
}