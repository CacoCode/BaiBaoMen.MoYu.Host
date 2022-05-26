using AutoMapper;

namespace Magicodes.Admin.Application.App
{
    public class AppDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //configuration.CreateMap<ChargeOption, ChargeOptionDto>()
            //    .ForMember(dto => dto.Rate, options => options.MapFrom(p => Math.Round((p.Duration / p.Amount), 2)));

        }
    }
}
