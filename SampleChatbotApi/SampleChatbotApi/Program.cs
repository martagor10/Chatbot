using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SampleChatbotApi;
using SampleChatbotApi.CQRS.Query;
using SampleChatbotApi.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services
    .AddOpenApi()
    .AddSwaggerGen(opt => { opt.OperationFilter<UsernameHeaderParameter>(); })
    .AddStorage(builder.Configuration)
    .AddAppServices()
    .AddChatIntegration(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            policyBuilder => policyBuilder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .SetIsOriginAllowed(_ => true)
        );
    });   
}

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetMessagesQuery>());

var app = builder.Build();

{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApi v1");
        opt.RoutePrefix = string.Empty;
    });
    
    app.UseCors("AllowAll");
}

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MessagesContext>();
    await db.Database.MigrateAsync();
}

app.Run();