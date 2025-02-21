using Microsoft.EntityFrameworkCore;
using SampleChatbotApi.Configuration;
using SampleChatbotApi.Service.Chat;
using SampleChatbotApi.Service.Message;
using SampleChatbotApi.Service.User;
using SampleChatbotApi.Storage;
// ReSharper disable UnusedMethodReturnValue.Global

namespace SampleChatbotApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
        => services
            .AddHttpContextAccessor()
            .AddScoped<IUserProvider, UserProvider>()
            .AddScoped<IMessageService, MessageService>()
            .AddAutoMapper(typeof(Program));

    public static IServiceCollection AddChatIntegration(this IServiceCollection services,
        IConfigurationManager configuration)
        => services.AddScoped<IChatbotService, ChatbotService>()
            .Configure<LocalChatServiceConfiguration>(
                configuration.GetSection(nameof(LocalChatServiceConfiguration)));

    public static IServiceCollection AddStorage(this IServiceCollection services,
        IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("MessagesDatabase");

        return services.AddDbContext<MessagesContext>(opt => opt.UseSqlServer(connectionString));
    }
}