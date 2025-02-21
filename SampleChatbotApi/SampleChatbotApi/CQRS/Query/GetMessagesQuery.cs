using MediatR;
using SampleChatbotApi.Api.Model;

namespace SampleChatbotApi.CQRS.Query;

public class GetMessagesQuery : IRequest<IEnumerable<MessageDto>>;