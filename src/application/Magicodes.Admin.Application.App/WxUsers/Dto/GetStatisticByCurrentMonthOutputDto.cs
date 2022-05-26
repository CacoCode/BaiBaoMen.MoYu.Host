using System;

namespace Magicodes.Admin.Application.App.WxUsers.Dto
{
    public class GetStatisticByCurrentMonthOutputDto
    {
        public GetStatisticByCurrentMonthOutputDto()
        {
        }

        public GetStatisticByCurrentMonthOutputDto(string wxUserAvatarUrl)
        {
            WxUserAvatarUrl = wxUserAvatarUrl;
        }

        public long WxUserId { get; set; }

        public string NickName { get; set; } = "虚位以待";

        public string WxUserAvatarUrl { get; set; }

        public string WxUserAvatarUrlStyle => $"background-image:url({WxUserAvatarUrl})";

        public int Count { get; set; } = 0;

        public int Integral { get; set; } = 0;
    }
}
