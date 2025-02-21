using MediatR;

namespace SampleChatbotApi.CQRS.Command;

internal class AddNewMessageCommand : IRequest<NewMessages>
{
    public required string Message { get; init; }
}