using Magicodes.Admin.Core.Custom.MoYuRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.Admin.Core.Custom.WxUsers
{
    public class WxUser : EntityBase<long>
    {
        public string OpenId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string AvatarUrl { get; set; }

        public ICollection<MoYuRecord> MoYuRecords { get; set; }
    }
}
