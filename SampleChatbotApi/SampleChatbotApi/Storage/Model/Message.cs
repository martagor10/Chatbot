using System.ComponentModel.DataAnnotations;

namespace SampleChatbotApi.Storage.Model;

public class Message
{
    public required Guid Id { get; init; }
    public Conversation? Conversation { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required MessageKind Kind { get; init; }
    [MaxLength(int.MaxValue)] public required string Text { get; set; }
    public MessageRating? Rating { get; set; }
}