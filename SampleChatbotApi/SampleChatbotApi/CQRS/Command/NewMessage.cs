namespace SampleChatbotApi.CQRS.Command;

internal record NewMessage(Guid Id, DateTimeOffset CreatedAt);