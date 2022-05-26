using Abp.UI;
using Magicodes.Admin.Core;
using Magicodes.AMap.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Magicodes.AMap
{
    public class AMapManager : IAMapManager
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly AMapOption _aMapOption;
        private readonly ILogger _logger;

        public AMapManager(IHttpClientFactory clientFactory,
            IOptions<AMapOption> aMapOption,
            ILogger<AMapManager> logger)
        {
            _clientFactory = clientFactory;
            _aMapOption = aMapOption.Value;;
            _logger = logger;
        }

        public async Task<List<string>> GetAdress(List<string> locations)
        {
            var client = _clientFactory.CreateClient(AdminConsts.AMapHttpClientName);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var str = string.Join('|', locations);
            var url = $"/v3/geocode/regeo?key={_aMapOption.Key}&location={str}&extensions=base&batch=true&output=json";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<AMapRegeoOutput>(responseStr);
                if (json.Status == "1")
                {
                    return json.Regeocodes.Select(a => a.Formatted_address).ToList();
                }
                else {
                    _logger.LogError(json.Info);
                    return new List<string>();
                }
            }
            else
            {
                _logger.LogError($"请求高德接口报错");
                return new List<string>();
            }
        }

        public async Task<RegeoCode> GetAdress(string location)
        {
            var client = _clientFactory.CreateClient(AdminConsts.AMapHttpClientName);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var url = $"/v3/geocode/regeo?key={_aMapOption.Key}&location={location}&extensions=base&batch=false&output=json";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<AMapRegeoOutput>(responseStr);
                if (json.Status == "1")
                {
                    return json.Regeocode;
                }
                else
                {
                    _logger.LogError(json.Info);
                    return new RegeoCode();
                }
            }
            else
            {
                _logger.LogError($"请求高德接口报错");
                return new RegeoCode();
            }
        }
    }
}
