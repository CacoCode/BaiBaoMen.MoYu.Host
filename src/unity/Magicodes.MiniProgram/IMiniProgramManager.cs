using System.Threading.Tasks;
using Abp.Dependency;
using Magicodes.MiniProgram.Dto;

namespace Magicodes.MiniProgram
{
    public interface IMiniProgramManager : ISingletonDependency
    {
        Task<GetSnsInfoByCodeOutput> JscodeToSession(string code);

        Task<MsgSecCheckOutputDto> CheckMessage(string openId, string contentText);
    }
}
