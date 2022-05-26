using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Magicodes.Admin.Core.Custom.MoYuRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Extensions;

namespace Magicodes.Admin.Application.App.MoYuRecords.Dto
{
    [AutoMapFrom(typeof(MoYuRecord))]
    public class MoYuRecordDto:EntityDto<long>
    {
        public long WxUserId { get; set; }
        public string WxUserNickName { get; set; }
        public string WxUserAvatarUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Desc { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime MoYuRealTime { get; set; }
        public MoYuTypeEnum MoYuType { get; set; }
        public string Date => MoYuRealTime.ToString("MM-dd");
        public string Hour => MoYuRealTime.ToString("HH:mm:ss");
        public string WxUserAvatarUrlStyle => $"background-image:url({WxUserAvatarUrl})";
        public string ImageUrlStyle => $"background-image:url({ImageUrl})";
        public string TogetherWxUsers { get; set; }

        public IList<long> TogetherWxUserIds => TogetherWxUsers.IsNullOrWhiteSpace()
            ? new List<long>()
            : TogetherWxUsers.Split(',').Select(long.Parse).ToList();

        public IList<string> TogetherWxUserAvatarUrl { get; set; }
        public bool IsShow { get; set; }
    }
}
