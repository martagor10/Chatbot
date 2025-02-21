namespace SampleChatbotApi.Exception;

public class MessageNotFoundException(Guid messageId) : System.Exception($"Message with Id = {messageId} not found");