using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Magicodes.MiniProgram.Dto
{
    public class GetTokenOutput : ServiceOutputBase
    {
        /// <summary>
        ///     获取到的凭证
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        ///     凭证有效时间，单位：秒。目前是7200秒之内的值。
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
