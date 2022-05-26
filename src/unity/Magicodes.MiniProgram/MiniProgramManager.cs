using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Abp.Runtime.Caching;
using Magicodes.MiniProgram.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace Magicodes.MiniProgram
{
    public class MiniProgramManager : IMiniProgramManager
    {
        private readonly HttpClient _httpClient;
        private readonly MiniProgramOption _miniProgramOption;
        private readonly IHttpContextAccessor _accessor;
        protected const string BaseApiUrl = "https://api.weixin.qq.com";
        private readonly ICacheManager _cacheManager;


        public MiniProgramManager(IHttpClientFactory clientFactory
            , IOptions<MiniProgramOption> miniProgramOption,
            IHttpContextAccessor accessor, 
            ICacheManager cacheManager)
        {
            _httpClient = clientFactory.CreateClient();
            _miniProgramOption = miniProgramOption.Value;
            _accessor = accessor;
            _cacheManager = cacheManager;
        }

        /// <summary>
        ///     根据登录凭证获取Sns信息（openid、session_key、unionid）
        /// https://developers.weixin.qq.com/miniprogram/dev/api-backend/open-api/login/auth.code2Session.html
        /// </summary>
        /// <param name="code">登录时获取的 code</param>
        public async Task<GetSnsInfoByCodeOutput> JscodeToSession(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("参数不能为空！", nameof(code));
            var url =
                $"{BaseApiUrl}/sns/jscode2session?appid={_miniProgramOption.AppId}&secret={_miniProgramOption.AppSecret}&js_code={code}&grant_type=authorization_code";

            var getTicket = await _httpClient.GetAsync(url);
            var responseBody = await getTicket.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetSnsInfoByCodeOutput>(responseBody);
        }


        public async Task<MsgSecCheckOutputDto> CheckMessage(string openId,string contentText)
        {
            var accessToken = await _cacheManager.GetCache("WeChat").GetAsync("AccessToken",GetToken);
            var url =
                $"{BaseApiUrl}/wxa/msg_sec_check?access_token={accessToken.AccessToken}";
            var dic = new Dictionary<string, string>
            {
                {"version", "2"},
                {"openid", openId},
                {"scene", "4"},
                {"content", contentText}
            };
            var dicString = JsonConvert.SerializeObject(dic);
            var content = new StringContent(dicString, Encoding.UTF8, "application/json");
            var getTicket = await _httpClient.PostAsync(url, content);
            var responseBody = await getTicket.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MsgSecCheckOutputDto>(responseBody);
            return result;
        }

        private async Task<GetTokenOutput> GetToken()
        {
            var url =
                $"{BaseApiUrl}/cgi-bin/token?grant_type=client_credential&appid={_miniProgramOption.AppId}&secret={_miniProgramOption.AppSecret}";
            var getTicket = await _httpClient.GetAsync(url);
            var responseBody = await getTicket.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetTokenOutput>(responseBody);
            await _cacheManager.GetCache("WeChat").SetAsync("AccessToken", result.AccessToken,TimeSpan.FromSeconds(6000));
            return result;
        }

    }
}
