using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Magicodes.Admin.Core.Custom.MoYuRecords;

namespace Magicodes.Admin.Application.Custom.MoYuRecords.Dto
{
    [AutoMapFrom(typeof(MoYuRecord))]
    public class MoYuRecordListDto:EntityDto<long>
    {
        public long WxUserId { get; set; }
        public string WxUserNickName { get; set; }
        public string WxUserAvatarUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Desc { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime MoYuRealTime { get; set; }
        public MoYuTypeEnum MoYuType { get; set; }
        public int Integral { get; set; }
        public bool IsWorkTime { get; set; }
        public string Date => MoYuRealTime.ToString("MM-dd");
        public string Hour => MoYuRealTime.ToString("HH:mm:ss");
    }
}
