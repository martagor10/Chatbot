using Microsoft.EntityFrameworkCore;
using SampleChatbotApi.Exception;
using SampleChatbotApi.Service.User;
using SampleChatbotApi.Storage;
using SampleChatbotApi.Storage.Model;

namespace SampleChatbotApi.Service.Message;

public class MessageService(MessagesContext context, IUserProvider userProvider) : IMessageService
{
    public async Task<Storage.Model.Message> InsertMessage(string text, MessageKind kind)
    {
        var conversation = await context.Conversations.FirstOrDefaultAsync(x => x.UserName == userProvider.CurrentUser);

        if (conversation is null)
        {
            conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                UserName = userProvider.CurrentUser,
                Messages = []
            };

            await context.Conversations.AddAsync(conversation);
        }

        var message = new Storage.Model.Message
        {
            Id = Guid.NewGuid(),
            Conversation = conversation,
            CreatedAt = DateTimeOffset.Now,
            Kind = kind,
            Text = text
        };

        await context.Messages.AddAsync(message);

        await context.SaveChangesAsync();
        
        return message;
    }
    
    public async Task<Storage.Model.Message> UpdateMessage(Guid id, string text)
    {
        var message = await context.Messages
            .Include(x => x.Conversation)
            .FirstOrDefaultAsync(x => x.Id == id);

        VerifyMessage(id, message);

        message!.Text = text;

        await context.SaveChangesAsync();

        return message;
    }

    public async Task<IEnumerable<Storage.Model.Message>> GetMessages()
    {
        var conversation = await context.Conversations
            .Include(conversation => conversation.Messages)
            .FirstOrDefaultAsync(x => x.UserName == userProvider.CurrentUser);

        return conversation == null ? [] : conversation.Messages.OrderBy(x => x.CreatedAt);
    }

    public async Task RateMessage(Guid id, MessageRating rating)
    {
        var message = await context.Messages
            .Include(x => x.Conversation)
            .FirstOrDefaultAsync(x => x.Id == id);

        VerifyMessage(id, message);

        message!.Rating = rating;

        await context.SaveChangesAsync();
    }
    
    private void VerifyMessage(Guid id, Storage.Model.Message? message)
    {
        if (message is null) throw new MessageNotFoundException(id);

        if (message.Conversation!.UserName != userProvider.CurrentUser)
            throw new UserUnauthorizedException(id, userProvider.CurrentUser);
    }
}