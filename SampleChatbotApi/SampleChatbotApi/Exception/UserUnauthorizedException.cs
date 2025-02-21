namespace SampleChatbotApi.Exception;

public class UserUnauthorizedException(Guid messageId, string userName)
    : System.Exception($"User {userName} is not authorized to perform operation on message with Id = {messageId}");