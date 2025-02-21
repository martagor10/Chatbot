using AutoMapper;
using JetBrains.Annotations;
using SampleChatbotApi.Api.Model;
using SampleChatbotApi.Storage.Model;

namespace SampleChatbotApi.Configuration;

[UsedImplicitly]
internal class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Message, MessageDto>()
            .ForCtorParam(nameof(MessageDto.Id), opt => opt.MapFrom(x => x.Id.ToString()))
            .ForCtorParam(nameof(MessageDto.CreatedAt), opt => opt.MapFrom(x => x.CreatedAt))
            .ForCtorParam(nameof(MessageDto.Text), opt => opt.MapFrom(x => x.Text))
            .ForCtorParam(nameof(MessageDto.Kind), opt => opt.MapFrom(x => x.Kind))
            .ForCtorParam(nameof(MessageDto.Rating), opt => opt.MapFrom(x => x.Rating));
    }
}