using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using SampleChatbotApi.Api.Model;
using SampleChatbotApi.Service.Message;

namespace SampleChatbotApi.CQRS.Query;

[UsedImplicitly]
public class GetMessagesQueryHandler(IMessageService messageService, IMapper mapper) : IRequestHandler<GetMessagesQuery, IEnumerable<MessageDto>>
{
    public async Task<IEnumerable<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await messageService.GetMessages();

        return mapper.Map<IEnumerable<MessageDto>>(messages);
    }
}