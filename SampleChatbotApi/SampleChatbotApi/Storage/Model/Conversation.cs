using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SampleChatbotApi.Storage.Model;

[Index(nameof(UserName), IsUnique = true)]
public class Conversation
{
    public required Guid Id { get; init; }

    [MaxLength(255)] public required string UserName { get; init; }

    public required ICollection<Message> Messages { get; init; }
}