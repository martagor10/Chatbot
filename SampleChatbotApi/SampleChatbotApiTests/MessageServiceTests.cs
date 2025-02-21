using NSubstitute;
using SampleChatbotApi.Service.Message;
using SampleChatbotApi.Service.User;
using SampleChatbotApi.Storage.Model;
using Xunit;

namespace SampleChatbotApiTests;

public class MessageServiceTests : TestBase
{
    private readonly IUserProvider _userProvider = Substitute.For<IUserProvider>();
    private readonly MessageService _messageService;
    private const string Username = "Test";

    public MessageServiceTests()
    {
        _userProvider.CurrentUser.Returns(Username);
        _messageService = new MessageService(MessagesContext, _userProvider);
    }

    [Theory]
    [InlineData(MessageRating.Positive)]
    [InlineData(MessageRating.Negative)]
    public async Task Given_MessageId_When_RatingMessage_Then_CorrectRatingIsSavedToDb(MessageRating rating)
    {
        //given
        var messageId = MessagesContext.Messages.First().Id;

        //when
        await _messageService.RateMessage(messageId, rating);
        var updatedMessage = MessagesContext.Messages.FirstOrDefault(x => x.Id == messageId);

        //then
        Assert.Equal(updatedMessage?.Rating, rating);
    }

    [Fact]
    public async Task Given_Messages_When_GettingMessages_Then_CorrectMessagesAreReturned()
    {
        //when
        var messages = await _messageService.GetMessages();

        //then
        Assert.Equal(messages.Select(x => x.Id),
            MessagesContext.Messages.Where(x => x.Conversation!.UserName == Username).OrderBy(x => x.CreatedAt)
                .Select(x => x.Id));
    }

    [Fact]
    public async Task Given_MessageIdAndText_When_UpdateMessage_Then_MessageIsUpdated()
    {
        //given
        var messageId = MessagesContext.Messages.First().Id;
        const string text = "This is a test";

        //when
        await _messageService.UpdateMessage(messageId, text);
        var message = MessagesContext.Messages.FirstOrDefault(x => x.Id == messageId);

        //then
        Assert.Equal(message?.Text, text);
    }

    [Fact]
    public async Task Given_MessageTextAndKind_When_InsertMessage_Then_MessageIsCreated()
    {
        //given
        const string testText = "This is a test2";
        const MessageKind messageKind = MessageKind.User;
        var messageCount = MessagesContext.Messages.Count();

        //when
        var createdMessage = await _messageService.InsertMessage(testText, messageKind);
        
        //then
        Assert.Equal(MessagesContext.Messages.Count(), messageCount+1);
        Assert.Equal(createdMessage.Text, testText);
        Assert.Equal(createdMessage.Kind, messageKind);
    }
}