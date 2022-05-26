using System;
using System.Linq;
using AutoMapper;
using Magicodes.Admin.Application.Custom.WxUsers.Dto;
using Magicodes.Admin.Core.Custom.WxUsers;

namespace Magicodes.Admin.Application.Custom.AutoMapper
{
    internal static class AdminAppDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //TODO:用户自定义映射
            configuration.CreateMap<WxUser, WxUserListDto>()
                .ForMember(dto => dto.MoYuRecords, options => options.MapFrom(p => p.MoYuRecords.Where(a => a.MoYuRealTime.Month == DateTime.Now.Month && a.MoYuRealTime.Year == DateTime.Now.Year).OrderByDescending(a => a.MoYuRealTime)));
            //configuration.CreateMap<UpdateChargeOptionInput, ChargeOption>();
            //configuration.CreateMap<CreateChargeTemplateInputDto, ChargeTemplate>();
            //configuration.CreateMap<UpdateChargeTemplateInput, ChargeTemplate>();
            //configuration.CreateMap<CreateEquipmentInputDto, Equipment>();
            //configuration.CreateMap<UpdateEquipmentInput, Equipment>();
            //configuration.CreateMap<ChargeTemplate, ChargeTemplateListDto>()
            //    .ForMember(dto => dto.ChargeOptionCount, options => options.MapFrom(p => p.ChargeOptions.Count));

        }
    }
}