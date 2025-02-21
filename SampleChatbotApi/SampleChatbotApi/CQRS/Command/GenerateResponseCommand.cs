using MediatR;

namespace SampleChatbotApi.CQRS.Command;

internal class GenerateResponseCommand : IStreamRequest<string>
{
    public required string Message { get; init; }
    public required Guid ResponseMessageId { get; init; }
}