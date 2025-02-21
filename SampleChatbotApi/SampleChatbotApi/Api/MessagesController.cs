using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SampleChatbotApi.Api.Model;
using SampleChatbotApi.CQRS.Command;
using SampleChatbotApi.CQRS.Query;
using SampleChatbotApi.Storage.Model;

namespace SampleChatbotApi.Api;

[ApiController]
[RequireUserHeader]
[Route("[controller]")]
public class MessagesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("new")]
    [ProducesResponseType<string>(statusCode: 200)]
    public async Task NewMessage(IncomingMessageDto message, CancellationToken cancellationToken)
    {
        var newMessageCommand = new AddNewMessageCommand
        {
            Message = message.Text
        };

        var newMessages = await mediator.Send(newMessageCommand, cancellationToken);

        Response.ContentType = "text/plain";
        Response.StatusCode = 200;

        Response.Headers.AccessControlExposeHeaders = new StringValues([
            CustomHeaders.ChatbotMessageId,
            CustomHeaders.ChatbotMessageDate,
            CustomHeaders.UserMessageId,
            CustomHeaders.UserMessageDate
        ]);
        Response.Headers.Append(CustomHeaders.UserMessageId, newMessages.User.Id.ToString());
        Response.Headers.Append(CustomHeaders.UserMessageDate, newMessages.User.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        Response.Headers.Append(CustomHeaders.ChatbotMessageId, newMessages.Chatbot.Id.ToString());
        Response.Headers.Append(CustomHeaders.ChatbotMessageDate, newMessages.Chatbot.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));

        await using var sw = new StreamWriter(Response.Body);

        var generateResponseCommand = new GenerateResponseCommand
        {
            Message = message.Text,
            ResponseMessageId = newMessages.Chatbot.Id
        };

        await foreach (var partialResponse in mediator.CreateStream(generateResponseCommand, cancellationToken)
                           .ConfigureAwait(false))
        {
            await sw.WriteAsync(partialResponse).ConfigureAwait(false);
            await sw.FlushAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }

    [HttpPut]
    [Route("{messageId:guid}/upvote")]
    public async Task<IActionResult> UpvoteMessage(Guid messageId)
    {
        var command = new RateMessageCommand
        {
            Id = messageId,
            Rating = MessageRating.Positive
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut]
    [Route("{messageId:guid}/downvote")]
    public async Task<IActionResult> DownvoteMessage(Guid messageId)
    {
        var command = new RateMessageCommand
        {
            Id = messageId,
            Rating = MessageRating.Negative
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    [Route("history")]
    [ProducesResponseType<IEnumerable<MessageDto>>(statusCode: 200)]
    public async Task<IActionResult> GetMessages()
    {
        var request = new GetMessagesQuery();
        return Ok(await mediator.Send(request));
    }
}