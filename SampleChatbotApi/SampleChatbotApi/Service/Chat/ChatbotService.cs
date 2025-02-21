using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using SampleChatbotApi.Configuration;
using SampleChatbotApi.Resource;

namespace SampleChatbotApi.Service.Chat;

internal class ChatbotService(IOptions<LocalChatServiceConfiguration> configuration) : IChatbotService
{
    private static readonly string[] Responses =
        [ChatbotServiceResponses.Short, ChatbotServiceResponses.Medium, ChatbotServiceResponses.Long];

    public async IAsyncEnumerable<string> GetResponse(string userInput,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var responseNumber = userInput.Sum(x => x) % Responses.Length;

        var response = Responses[responseNumber];

        foreach (var word in response.Split(" "))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            yield return $"{word} ";

            await Task.Delay(configuration.Value.ResponseDelayMilliseconds, cancellationToken);
        }
    }
}