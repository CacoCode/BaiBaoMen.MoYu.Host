using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.MiniProgram.Dto
{
    public class MsgSecCheckOutputDto:ServiceOutputBase
    {
        public MsgSecCheckResult Result { get; set; }
    }

    public class MsgSecCheckResult
    {
        public string Suggest { get; set; }
        public int Label { get; set; }

        public string Text()
        {
            switch (Label)
            {
                case 10001:
                    return "广告";
                case 20001:
                    return "时政";
                case 20002:
                    return "色情";
                case 20003:
                    return "辱骂";
                case 20006:
                    return "违法犯罪";
                case 20008:
                    return "欺诈";
                case 20012:
                    return "低俗";
                case 20013:
                    return "版权";
                case 21000:
                    return "其他";
                default: return "正常";
            }
        }
    }
}
