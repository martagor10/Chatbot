using SampleChatbotApi.Storage.Model;

namespace SampleChatbotApi.Api.Model;

public record MessageDto(string Id,
    DateTimeOffset CreatedAt,
    string Text,
    MessageKind Kind,
    MessageRating? Rating);