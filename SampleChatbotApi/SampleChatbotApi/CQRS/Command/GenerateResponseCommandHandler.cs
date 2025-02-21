using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using MediatR;
using SampleChatbotApi.Service.Chat;
using SampleChatbotApi.Service.Message;

namespace SampleChatbotApi.CQRS.Command;

[UsedImplicitly]
internal class GenerateResponseCommandHandler(
    IMessageService messageService,
    IChatbotService chatbotService)
    : IStreamRequestHandler<GenerateResponseCommand, string>
{
    public async IAsyncEnumerable<string> Handle(GenerateResponseCommand request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var responseMessageBuilder = new StringBuilder();
        
        try
        {
            await foreach (var partialResponse in chatbotService.GetResponse(request.Message, cancellationToken)
                               .ConfigureAwait(false))
            {
                yield return partialResponse;

                responseMessageBuilder.Append(partialResponse);
            }
        }
        finally
        {
            await messageService.UpdateMessage(request.ResponseMessageId, responseMessageBuilder.ToString());
        }
    }
}