using Abp.AutoMapper;
using Abp.Modules;
using AutoMapper;
using Magicodes.Admin.Core.Custom;
using Magicodes.AMap;
using System;
using System.Reflection;
using Magicodes.MiniProgram;

namespace Magicodes.Admin.Application.App
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AppCoreModule),
        typeof(MiniProgramModule),
        typeof(AMapModule)
    )]
    public class ApplicationAppModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add((Action<IMapperConfigurationExpression>)AppDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
