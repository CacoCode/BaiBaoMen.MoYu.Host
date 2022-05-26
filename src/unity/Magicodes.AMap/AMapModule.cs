using Abp.Modules;
using Abp.Reflection.Extensions;
using Magicodes.Admin.Core;
using System;

namespace Magicodes.AMap
{
    [DependsOn(typeof(AdminCoreModule))]
    public class AMapModule : AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AMapModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }
    }
}
