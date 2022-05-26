using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Magicodes.Admin.Application.App.MoYuRecords.Dto;
using Magicodes.Admin.Core.Custom.WxUsers;

namespace Magicodes.Admin.Application.App.WxUsers.Dto
{
    [AutoMapFrom(typeof(WxUser))]
    public class WxUserDto : EntityDto<long>
    {
        public string OpenId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string AvatarUrl { get; set; }
        public int MoYuCount { get; set; } = 0;
        public int Integral { get; set; } = 0;
        public int Rank { get; set; } = 9999;

        public string SessionKey { get; set; }
    }

    [AutoMapFrom(typeof(WxUser))]
    public class WxUserCheckDto : EntityDto<long>
    {
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsChecked { get; set; } = false;
        public IList<MoYuRecordListDto> MoYuRecords { get; set; }
        public int CurrentIntegral => MoYuRecords.Where(a => a.MoYuRealTime.Date == DateTime.Now.Date && a.WxUserId == Id).Select(a=>a.Integral).Sum();

        public bool IsLocked => CurrentIntegral >= 2;
    }

    [AutoMapFrom(typeof(WxUser))]
    public class WxUserAvatarDto : EntityDto<long>
    {
        public string AvatarUrl { get; set; }
    }
}
