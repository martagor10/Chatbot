using SampleChatbotApi.Storage.Model;

namespace SampleChatbotApi.Service.Message;

public interface IMessageService
{
    Task<Storage.Model.Message> InsertMessage(string text, MessageKind kind);
    Task<Storage.Model.Message> UpdateMessage(Guid id, string text);
    Task<IEnumerable<Storage.Model.Message>> GetMessages();
    Task RateMessage(Guid id, MessageRating rating);
}