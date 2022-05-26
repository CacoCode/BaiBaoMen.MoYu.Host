using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using Magicodes.Admin.Application.App.MoYuRecords.Dto;
using Magicodes.Admin.Core.Custom.MoYuRecords;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Magicodes.Admin.Application.App.WxUsers.Dto;
using Magicodes.Admin.Core.Custom.WxUsers;
using Magicodes.MiniProgram;
using Microsoft.EntityFrameworkCore;

namespace Magicodes.Admin.Application.App.MoYuRecords
{
    public class MoYuRecordAppService : AppServiceBase, IMoYuRecordAppService
    {
        private readonly IRepository<MoYuRecord, long> _moYuRecordRepository;
        private readonly HttpClient _httpClient;
        private readonly IMiniProgramManager _miniProgramManager;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<WxUser, long> _wxUserRepository;



        public MoYuRecordAppService(IRepository<MoYuRecord, long> moYuRecordRepository, 
            IHttpClientFactory clientFactory, IMiniProgramManager miniProgramManager, 
            ICacheManager cacheManager, IRepository<WxUser, long> wxUserRepository)
        {
            _moYuRecordRepository = moYuRecordRepository;
            _miniProgramManager = miniProgramManager;
            _cacheManager = cacheManager;
            _wxUserRepository = wxUserRepository;
            _httpClient = clientFactory.CreateClient();
        }

        /// <summary>
        /// Creates the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException">
        /// 文本中含有违法违规内容
        /// or
        /// 文本中含有{checkResult.Result.Text()}敏感内容
        /// or
        /// 必须上传图片
        /// or
        /// 今日打卡积分已满，不能再打卡
        /// </exception>
        [AbpAllowAnonymous]
        public async Task<MoYuRecordDto> Create(CreateMoYuRecordDto input)
        {
            if (!input.Desc.IsNullOrWhiteSpace())
            {
                var checkResult = await _miniProgramManager.CheckMessage(input.OpenId, input.Desc);
                if (!checkResult.IsSuccess()) throw new UserFriendlyException("文本中含有违法违规内容");
                if (checkResult.Result.Label != 100) throw new UserFriendlyException($"文本中含有{checkResult.Result.Text()}敏感内容");
            }
            if (string.IsNullOrWhiteSpace(input.ImageUrl)) throw new UserFriendlyException("必须上传图片");
            var list = await _moYuRecordRepository.GetAllListAsync(a => a.WxUserId == input.WxUserId && a.MoYuRealTime.Date == DateTime.Now.Date);
            if (list.Count > 0 && list.Select(a => a.Integral).Sum() == 2) throw new UserFriendlyException("今日打卡积分已满，不能再打卡");
            var newMoYuRecord = new MoYuRecord()
            {
                WxUserId = input.WxUserId,
                ImageUrl = input.ImageUrl,
                Desc = input.Desc,
                MoYuRealTime = input.MoYuRealTime ?? Clock.Now,
                Integral = await GetIntegral(input.MoYuRealTime ?? Clock.Now, list.Sum(a => a.Integral)),
                CreationTime = Clock.Now,
                MoYuType = MoYuTypeEnum.Photo,
                IsWorkTime = await IsWorkTime(Clock.Now),
                
            };
            if (!input.TogetherWxUsers.IsNullOrWhiteSpace())
            {
                var togetherWxUserIds = input.TogetherWxUsers.Split(",").Select(long.Parse).ToList();
                if (togetherWxUserIds.Any())
                {
                    newMoYuRecord.TogetherWxUsers = input.TogetherWxUsers;
                    foreach (var togetherWxUserId in togetherWxUserIds)
                    {
                        var togetherMoYuRecord = new MoYuRecord()
                        {
                            WxUserId = togetherWxUserId,
                            ImageUrl = input.ImageUrl,
                            Desc = input.Desc,
                            MoYuRealTime = input.MoYuRealTime ?? Clock.Now,
                            Integral = await GetIntegral(input.MoYuRealTime ?? Clock.Now, list.Sum(a => a.Integral)),
                            CreationTime = Clock.Now,
                            MoYuType = MoYuTypeEnum.Photo,
                            IsWorkTime = await IsWorkTime(Clock.Now),
                            IsShow = false
                        };
                        await _moYuRecordRepository.InsertAsync(togetherMoYuRecord);
                    }
                }
            }
            
            var model = await _moYuRecordRepository.InsertAsync(newMoYuRecord);
            return ObjectMapper.Map<MoYuRecordDto>(model);
        }

        private void CheckWxUserCanMoYu(List<MoYuRecord> list,long wxUserId)
        {
            if (list.Count(a => a.WxUserId == wxUserId) > 0 &&
                list.Where(a => a.WxUserId == wxUserId)
                    .Select(a => a.Integral).Sum() == 2) 
                throw new UserFriendlyException("今日打卡积分已满，不能再打卡");
        }

        /// <summary>
        /// Gets the mo yu records.
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<MoYuRecordDto>> GetMoYuRecords()
        {
            var data = await _moYuRecordRepository.GetAllIncluding(a => a.WxUser).Where(a=>a.IsShow)
                .OrderByDescending(a => a.CreationTime).Take(100).ToListAsync();
            var result = ObjectMapper.Map<List<MoYuRecordDto>>(data);
            var temp = result.Where(a=>a.TogetherWxUserIds.Any());
            var userAvatars = await _cacheManager.GetCache("Users").GetAsync("UserAvatar", SetUserAvatar);
            foreach (var item in temp)
            {
                item.TogetherWxUserAvatarUrl = userAvatars.Where(a => item.TogetherWxUserIds.Contains(a.Id))
                    .Select(a => a.AvatarUrl).ToList();
            }

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
        /// Posts the step.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException">
        /// 今日打卡积分已满，不能再上报
        /// or
        /// 今日运动步数未满10000步，不能上报
        /// </exception>
        [AbpAllowAnonymous]
        public async Task<MoYuRecordDto> PostStep(PostStepInputDto input)
        {
            var list = await _moYuRecordRepository.GetAllListAsync(a => a.WxUserId == input.WxUserId && a.MoYuRealTime.Date == DateTime.Now.Date);
            if (list.Count > 0 && list.Select(a => a.Integral).Sum() == 2) throw new UserFriendlyException("今日打卡积分已满，不能再上报");
            if (input.Step < 10000) throw new UserFriendlyException("今日运动步数未满10000步，不能上报");

            var newMoYuRecord = new MoYuRecord()
            {
                WxUserId = input.WxUserId,
                Desc = $"今日运动步数为 {input.Step} 步，打卡成功",
                MoYuRealTime = Clock.Now,
                Integral = list.Sum(a => a.Integral) > 0 ? 1 : 2,
                MoYuType = MoYuTypeEnum.Step,
                IsWorkTime = false,
                CreationTime = Clock.Now
            };
            var model = await _moYuRecordRepository.InsertAsync(newMoYuRecord);
            return ObjectMapper.Map<MoYuRecordDto>(model);
        }

        private async Task<bool> IsWorkTime(DateTime date)
        {
            var url = $"https://tool.bitefu.net/jiari/vip.php?d={date:yyyyMMdd}&apikey=o2qeotvqYgilG1K9";
            var response = await _httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var status = JsonConvert.DeserializeObject<GetDayStatusDto>(responseBody);
            if (status.status == 0) throw new UserFriendlyException("服务器内部错误：100001");
            switch (status.data)
            {
                case 1:
                case 2:
                    return false;
                default:
                    return date.Hour.IsIn(9, 10, 11, 14, 15, 16, 17);
            }
        }

        private async Task<int> GetIntegral(DateTime date, int currentIntegral)
        {
            if (await IsWorkTime(date))
            {
                return 1;
            }

            return currentIntegral > 0 ? 1 : 2;
        }


    }
}
