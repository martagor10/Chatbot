using JetBrains.Annotations;
using MediatR;
using SampleChatbotApi.Service.Message;

namespace SampleChatbotApi.CQRS.Command;

[UsedImplicitly]
public class RateMessageCommandHandler(IMessageService messageService) : IRequestHandler<RateMessageCommand>
{
    public async Task Handle(RateMessageCommand request, CancellationToken cancellationToken)
    {
        await messageService.RateMessage(request.Id, request.Rating);
    }
}