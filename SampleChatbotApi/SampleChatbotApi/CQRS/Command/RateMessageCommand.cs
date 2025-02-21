using MediatR;
using SampleChatbotApi.Storage.Model;

namespace SampleChatbotApi.CQRS.Command;

public class RateMessageCommand : IRequest
{
    public required MessageRating Rating { get; init; }
    public required Guid Id { get; init; }
}