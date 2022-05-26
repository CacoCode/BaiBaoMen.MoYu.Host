using Abp.Modules;
using Abp.Reflection.Extensions;
using Magicodes.Admin.Core;

namespace Magicodes.MiniProgram
{
    [DependsOn(
    typeof(AdminCoreModule))]
    public class MiniProgramModule : AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MiniProgramModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }
    }
}
