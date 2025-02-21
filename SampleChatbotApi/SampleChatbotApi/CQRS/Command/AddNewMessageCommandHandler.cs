using JetBrains.Annotations;
using MediatR;
using SampleChatbotApi.Service.Message;
using SampleChatbotApi.Storage.Model;

namespace SampleChatbotApi.CQRS.Command;

[UsedImplicitly]
internal class AddNewMessageCommandHandler(IMessageService messageService)
    : IRequestHandler<AddNewMessageCommand, NewMessages>
{
    public async Task<NewMessages> Handle(AddNewMessageCommand request, CancellationToken cancellationToken)
    {
        var questionMessage = await messageService.InsertMessage(request.Message, MessageKind.User);
        var responseMessage = await messageService.InsertMessage(string.Empty, MessageKind.Chatbot);

        return new NewMessages(
            new NewMessage(questionMessage.Id, questionMessage.CreatedAt),
            new NewMessage(responseMessage.Id, responseMessage.CreatedAt));
    }
}