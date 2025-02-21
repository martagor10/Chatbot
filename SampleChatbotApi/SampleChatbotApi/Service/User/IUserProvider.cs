namespace SampleChatbotApi.Service.User;

public interface IUserProvider
{
    string CurrentUser { get; }
}