using System;
using System.Collections.Generic;
using System.Text;
using Magicodes.Admin.Core.Custom.WxUsers;

namespace Magicodes.Admin.Core.Custom.MoYuRecords
{
    public class MoYuRecord : EntityBase<long>
    {
        public long WxUserId { get; set; }

        public virtual WxUser WxUser { get; set; }

        public string ImageUrl { get; set; }

        public string Desc { get; set; }

        public DateTime MoYuRealTime { get; set; }

        public int Integral { get; set; }

        public MoYuTypeEnum MoYuType { get; set; } = MoYuTypeEnum.Photo;

        public bool IsWorkTime { get; set; } = true;

        public string TogetherWxUsers { get; set; }

        public bool IsShow { get; set; } = true;

    }

    public enum MoYuTypeEnum
    {
        Photo,
        Step
    }
}
