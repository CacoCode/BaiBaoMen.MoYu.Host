using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.Admin.Application.App.WxUsers.Dto
{
    public class WeRunDataOutputDto
    {
        public int Step { get; set; }

        public long Timestamp { get; set; }
    }

    public class WeRunList
    {
        public List<WeRunDataOutputDto> StepInfoList { get; set; }
}
}
