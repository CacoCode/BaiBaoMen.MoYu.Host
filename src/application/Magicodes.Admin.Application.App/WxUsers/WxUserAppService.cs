using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Timing;
using Abp.UI;
using Magicodes.Admin.Application.App.WxUsers.Dto;
using Magicodes.Admin.Core.Configuration;
using Magicodes.Admin.Core.Custom.WxUsers;
using Magicodes.MiniProgram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Magicodes.Admin.Application.App.WxUsers
{
    public class WxUserAppService : AppServiceBase, IWxUserAppService
    {

        private readonly IConfigurationRoot _appConfiguration;
        private readonly ICacheManager _cacheManager;
        private readonly IMiniProgramManager _miniProgramManager;
        private readonly IRepository<WxUser, long> _wxUserRepository;
        private readonly HttpClient _httpClient;

        public WxUserAppService(IAppConfigurationAccessor configurationAccessor, ICacheManager cacheManager, IMiniProgramManager miniProgramManager,
            IRepository<WxUser, long> wxUserRepository, IHttpClientFactory clientFactory)
        {
            _appConfiguration = configurationAccessor.Configuration;
            _cacheManager = cacheManager;
            _miniProgramManager = miniProgramManager;
            _wxUserRepository = wxUserRepository;
            _httpClient = clientFactory.CreateClient();
        }

        /// <summary>
        /// Creates the wx user.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<WxUserDto> CreateWxUser(CreateWxUserDto input)
        {
            var output = await _miniProgramManager.JscodeToSession(input.Code);
            if (output.IsSuccess())
            {
                var wxUser = await _wxUserRepository.FirstOrDefaultAsync(a => a.OpenId == output.OpenId);
                if (wxUser == null)
                {
                    var newWxUser = new WxUser
                    {
                        OpenId = output.OpenId,
                        AvatarUrl = input.AvatarUrl,
                        NickName = input.NickName,
                        CreationTime = Clock.Now
                    };
                    var id = await _wxUserRepository.InsertAndGetIdAsync(newWxUser);
                    newWxUser.Id = id;
                    wxUser = newWxUser;
                }
                var result = ObjectMapper.Map<WxUserDto>(wxUser);
                result.SessionKey = output.SessionKey;
                var dic = await _cacheManager.GetCache("Users").GetOrDefaultAsync<string,List<WxUserAvatarDto>>("UserAvatar");
                if (dic == null || !dic.Any())
                {
                    dic = new List<WxUserAvatarDto>
                    {
                        new WxUserAvatarDto
                        {
                            Id = result.Id,
                            AvatarUrl = result.AvatarUrl
                        }
                    };
                }
                else
                {
                    dic.RemoveAll(a => a.Id == result.Id);
                    dic.AddIfNotContains(new WxUserAvatarDto
                    {
                        Id = result.Id,
                        AvatarUrl = result.AvatarUrl
                    });
                }

                await _cacheManager.GetCache("Users").SetAsync("UserAvatar", dic, TimeSpan.FromDays(999999));
                return result;
            }
            return new WxUserDto();
        }

        /// <summary>
        /// Wxes the login.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException">用户不存在</exception>
        [AbpAllowAnonymous]
        public async Task<WxUserDto> WxLogin(string code)
        {
            var output = await _miniProgramManager.JscodeToSession(code);
            var wxUser = await _wxUserRepository.FirstOrDefaultAsync(a => a.OpenId == output.OpenId);
            if (wxUser == null) throw new UserFriendlyException("用户不存在");
            var model = ObjectMapper.Map<WxUserDto>(wxUser);
            model.SessionKey = output.SessionKey;
            return model;
        }

        /// <summary>
        /// Gets the wx users.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<WxUserCheckDto>> GetWxUsers(long id)
        {
            var wxUsers = await _wxUserRepository
                .GetAllIncluding(a => a.MoYuRecords)
                .Where(a => a.Id != id)
                .ToListAsync();
            var models = ObjectMapper.Map<List<WxUserCheckDto>>(wxUsers);
            return models;
        }

        /// <summary>
        /// Gets the wx user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException">用户不存在</exception>
        [AbpAllowAnonymous]
        public async Task<WxUserDto> GetWxUser(long id)
        {
            var current = DateTime.Now;
            var wxUser = await _wxUserRepository.GetAllIncluding(a => a.MoYuRecords).FirstOrDefaultAsync(b => b.Id == id);
            if (wxUser == null) throw new UserFriendlyException("用户不存在");
            var sort = _wxUserRepository.GetAllIncluding(a => a.MoYuRecords)
                .Select(b => new
                {
                    b.Id,
                    Integral = b.MoYuRecords.Where(a =>
                            a.MoYuRealTime.Month == current.Month && a.MoYuRealTime.Year == current.Year)
                        .Sum(c => c.Integral)
                })
                .OrderByDescending(c => int.Parse(c.Integral.ToString()))
            .ThenBy(a => a.Id)
            .ToList();
            var rank = sort.FindIndex(a => a.Id == wxUser.Id) + 1;
            var model = ObjectMapper.Map<WxUserDto>(wxUser);
            model.Rank = rank;
            var moYuRecords = wxUser.MoYuRecords
                .Where(a => a.MoYuRealTime.Month == current.Month && a.MoYuRealTime.Year == current.Year).ToList();
            model.MoYuCount = moYuRecords.Count;
            model.Integral = moYuRecords.Sum(c => c.Integral);
            return model;
        }

        /// <summary>
        /// Gets the statistic by.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<GetStatisticByCurrentMonthOutputDto>> GetStatisticBy(int tag)
        {
            var current = DateTime.Now.AddMonths(tag);
            var query = _wxUserRepository.GetAllIncluding(a => a.MoYuRecords);
            var result = query.Select(b => new GetStatisticByCurrentMonthOutputDto
            {
                WxUserId = b.Id,
                NickName = b.NickName,
                Count = b.MoYuRecords.Count,
                WxUserAvatarUrl = b.AvatarUrl,
                Integral = b.MoYuRecords.Where(a => a.MoYuRealTime.Month == current.Month && a.MoYuRealTime.Year == current.Year).Sum(c => c.Integral)
            }).ToList();
            result = result.OrderByDescending(a => a.Integral).ThenBy(a => a.WxUserId).ToList();

            var temp = 10 - result.Count;
            if (temp > 0)
            {
                for (int i = 0; i < temp; i++)
                {
                    var defaultAvatar = _appConfiguration["StorageProvider:LocalStorageProvider:RootUrl"] + "/avatar.png";
                    result.Add(new GetStatisticByCurrentMonthOutputDto(defaultAvatar));
                }
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Updates the name of the nick.
        /// </summary>
        /// <param name="input">The input.</param>
        [AbpAllowAnonymous]
        public async Task UpdateNickName(UpdateWxUserDto input)
        {
            var wxUser = await _wxUserRepository.GetAsync(input.Id);
            wxUser.NickName = input.NickName;
            await _wxUserRepository.UpdateAsync(wxUser);
        }

        [AbpAllowAnonymous]
        public async Task<List<WxUserAvatarDto>> GetUserAvatar()
        {
            var result= await _cacheManager.GetCache("Users").GetAsync("UserAvatar", SetUserAvatar);
            return result;
        }

        private async Task<List<WxUserAvatarDto>> SetUserAvatar()
        {
            var users = await _wxUserRepository.GetAll().Select(a => new WxUserAvatarDto
            {
                Id = a.Id,
                AvatarUrl = a.AvatarUrl
            }).ToListAsync();
            await _cacheManager.GetCache("Users").SetAsync("UserAvatar", users, TimeSpan.FromDays(999999));
            return users;
        }

        /// <summary>
        /// Gets the current step.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<int> GetCurrentStep(WeRunDataInputDto input)
        {
            var rawData = AES_decrypt(input.EncryptedData, input.SessionKey, input.Iv);
            var step = 0;
            if (string.IsNullOrEmpty(rawData) == false)
            {
                var data = JsonConvert.DeserializeObject<WeRunList>(rawData);
                if (data.StepInfoList.Count > 0)
                {
                    var current = data.StepInfoList.OrderByDescending(a => a.Timestamp).FirstOrDefault();
                    if (current != null) step = current.Step;
                }
            }
            return await Task.FromResult(step);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptedDataStr"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static string AES_decrypt(string encryptedDataStr, string key, string iv)
        {
            try
            {

                RijndaelManaged rijalg = new RijndaelManaged();
                //-----------------      
                //设置 cipher 格式 AES-128-CBC      

                rijalg.KeySize = 128;

                rijalg.Padding = PaddingMode.PKCS7;
                rijalg.Mode = CipherMode.CBC;

                rijalg.Key = Convert.FromBase64String(key);
                rijalg.IV = Convert.FromBase64String(iv);


                byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);
                //解密      
                ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

                string result = null;

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {

                return e.Message;
            }


        }
    }
}
