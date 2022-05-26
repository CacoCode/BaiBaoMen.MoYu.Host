using Newtonsoft.Json;

namespace Magicodes.MiniProgram.Dto
{
    /// <summary>
    /// 输入参数基类
    /// </summary>
    public class ServiceInputBase
    {
        /// <summary>
        /// 	接口调用凭证
        /// </summary>
        [JsonProperty("access_token	")]
        public string AccessToken { get; set; }
    }
}
