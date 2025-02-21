using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using SampleChatbotApi.Api;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleChatbotApi;

[UsedImplicitly]
internal class UsernameHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = CustomHeaders.Username,
            In = ParameterLocation.Header,
            Description = null,
            Required = false,
            Deprecated = false,
            AllowEmptyValue = true
        });
    }
}