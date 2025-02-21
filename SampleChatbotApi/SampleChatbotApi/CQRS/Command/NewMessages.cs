namespace SampleChatbotApi.CQRS.Command;

internal record NewMessages(NewMessage User, NewMessage Chatbot);