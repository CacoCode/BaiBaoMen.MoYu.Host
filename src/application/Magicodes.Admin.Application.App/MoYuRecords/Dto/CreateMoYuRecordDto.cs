using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Magicodes.Admin.Application.App.MoYuRecords.Dto
{
    public class CreateMoYuRecordDto
    {
        [Required]
        public long WxUserId { get; set; }
        [Required]
        public string OpenId { get; set; }
        public string ImageUrl { get; set; }
        public string Desc { get; set; }
        public DateTime? MoYuRealTime { get; set; }
        public string TogetherWxUsers { get; set; }
    }
}
