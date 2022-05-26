using System;
using System.Collections.Generic;
using System.Linq;
using Abp.AutoMapper;
using Magicodes.Admin.Application.Custom.MoYuRecords.Dto;
using Magicodes.Admin.Core.Custom.WxUsers;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace Magicodes.Admin.Application.Custom.WxUsers.Dto
{
    [AutoMapFrom(typeof(WxUser))]
    [ExcelExporter(Name = "排名信息", TableStyle = "Light10")]
    public class ExportWxUserDto
    {
        [ExporterHeader(DisplayName = "名字", IsAutoFit = true)]
        public string NickName { get; set; }

        [ExporterHeader(DisplayName = "本月次数", IsAutoFit = true)]
        public int MoYuCount => MoYuRecords.Count;

        [ExporterHeader(DisplayName = "本月积分", IsAutoFit = true)]
        public int Integral => MoYuRecords
            .Where(a => a.MoYuRealTime.Month == DateTime.Now.Month && a.MoYuRealTime.Year == DateTime.Now.Year)
            .Sum(c => c.Integral);
        [ExporterHeader(IsIgnore = true)]
        public IList<MoYuRecordListDto> MoYuRecords { get; set; }
    }
}
