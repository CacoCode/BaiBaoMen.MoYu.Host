using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Magicodes.Admin.Application.Custom.MoYuRecords.Dto;
using Magicodes.Admin.Core.Custom.WxUsers;

namespace Magicodes.Admin.Application.Custom.WxUsers.Dto
{
    [AutoMapFrom(typeof(WxUser))]
    public class WxUserListDto : EntityDto<long>
    {
        public string OpenId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string AvatarUrl { get; set; }

        public int MoYuCount => MoYuRecords
            .Count(a => a.MoYuRealTime.Month == DateTime.Now.Month && a.MoYuRealTime.Year == DateTime.Now.Year);

        public int Integral => MoYuRecords
            .Where(a => a.MoYuRealTime.Month == DateTime.Now.Month && a.MoYuRealTime.Year == DateTime.Now.Year)
            .Sum(c => c.Integral);
        public IList<MoYuRecordListDto> MoYuRecords { get; set; }
    }
}
