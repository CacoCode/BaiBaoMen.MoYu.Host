using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Magicodes.Admin.Core.Custom.MoYuRecords;

namespace Magicodes.Admin.Application.App.MoYuRecords.Dto
{
    [AutoMapFrom(typeof(MoYuRecord))]
    public class MoYuRecordListDto : EntityDto<long>
    {
        public long WxUserId { get; set; }

        public DateTime MoYuRealTime { get; set; }

        public int Integral { get; set; }

        public MoYuTypeEnum MoYuType { get; set; } = MoYuTypeEnum.Photo;

        public bool IsWorkTime { get; set; } = true;
    }
}
