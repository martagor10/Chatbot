using SampleChatbotApi.Exception;

namespace SampleChatbotApi.Service.User;

public class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
{
    public string CurrentUser => httpContextAccessor.HttpContext?.User.Identity?.Name ?? throw new NoUserDataException();
}