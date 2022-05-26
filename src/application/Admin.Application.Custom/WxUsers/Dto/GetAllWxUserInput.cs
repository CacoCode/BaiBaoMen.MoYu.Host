using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Magicodes.Admin.Application.Core.Dto;

namespace Magicodes.Admin.Application.Custom.WxUsers.Dto
{
    public class GetAllWxUserInput : PagedAndSortedInputDto, IShouldNormalize, IPagedAndSortedResultRequest
    {
        public string FilterText { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
