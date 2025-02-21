namespace SampleChatbotApi.Service.Chat;

public interface IChatbotService
{
    IAsyncEnumerable<string> GetResponse(string userInput, CancellationToken cancellationToken = default);
}