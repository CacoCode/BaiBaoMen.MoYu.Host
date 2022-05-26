using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Magicodes.AMap.Dto;

namespace Magicodes.AMap
{
    public interface IAMapManager : ISingletonDependency
    {
        Task<List<string>> GetAdress(List<string> locations);

        Task<RegeoCode> GetAdress(string location);
    }
}
