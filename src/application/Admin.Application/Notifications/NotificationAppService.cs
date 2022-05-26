﻿// ======================================================================
// 
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司
//           All rights reserved
// 
//           filename : NotificationAppService.cs
//           description :
// 
//           created by 雪雁 at  2019-06-14 11:22
//           开发文档: docs.xin-lai.com
//           公众号教程：magiccodes
//           QQ群：85318032（编程交流）
//           Blog：http://www.cnblogs.com/codelove/
//           Home：http://xin-lai.com
// 
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Magicodes.Admin.Application.Core;
using Magicodes.Admin.Application.Notifications.Dto;

namespace Magicodes.Admin.Application.Notifications
{
    [AbpAuthorize]
    public class NotificationAppService : AdminAppServiceBase, INotificationAppService
    {
        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IUserNotificationManager _userNotificationManager;

        public NotificationAppService(
            INotificationDefinitionManager notificationDefinitionManager,
            IUserNotificationManager userNotificationManager,
            INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _notificationDefinitionManager = notificationDefinitionManager;
            _userNotificationManager = userNotificationManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        [DisableAuditing]
        public async Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input)
        {
            var totalCount = await _userNotificationManager.GetUserNotificationCountAsync(
                AbpSession.ToUserIdentifier(), input.State
            );

            var unreadCount = await _userNotificationManager.GetUserNotificationCountAsync(
                AbpSession.ToUserIdentifier(), UserNotificationState.Unread
            );

            var notifications = await _userNotificationManager.GetUserNotificationsAsync(
                AbpSession.ToUserIdentifier(), input.State, input.SkipCount, input.MaxResultCount
            );

            return new GetNotificationsOutput(totalCount, unreadCount, notifications);
        }

        public async Task SetAllNotificationsAsRead()
        {
            await _userNotificationManager.UpdateAllUserNotificationStatesAsync(AbpSession.ToUserIdentifier(),
                UserNotificationState.Read);
        }

        public async Task SetNotificationAsRead(EntityDto<Guid> input)
        {
            var userNotification =
                await _userNotificationManager.GetUserNotificationAsync(AbpSession.TenantId, input.Id);
            if (userNotification.UserId != AbpSession.GetUserId())
                throw new Exception(string.Format(
                    "Given user notification id ({0}) is not belong to the current user ({1})", input.Id,
                    AbpSession.GetUserId()));

            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId, input.Id,
                UserNotificationState.Read);
        }

        public async Task<GetNotificationSettingsOutput> GetNotificationSettings()
        {
            var output = new GetNotificationSettingsOutput();

            output.ReceiveNotifications =
                await SettingManager.GetSettingValueAsync<bool>(NotificationSettingNames.ReceiveNotifications);

            //Get general notifications, not entity related notifications.
            var notificationDefinitions =
                (await _notificationDefinitionManager.GetAllAvailableAsync(AbpSession.ToUserIdentifier())).Where(nd =>
                    nd.EntityType == null);

            output.Notifications =
                ObjectMapper.Map<List<NotificationSubscriptionWithDisplayNameDto>>(notificationDefinitions);

            var subscribedNotifications = (await _notificationSubscriptionManager
                    .GetSubscribedNotificationsAsync(AbpSession.ToUserIdentifier()))
                .Select(ns => ns.NotificationName)
                .ToList();

            output.Notifications.ForEach(n => n.IsSubscribed = subscribedNotifications.Contains(n.Name));

            return output;
        }

        public async Task UpdateNotificationSettings(UpdateNotificationSettingsInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(),
                NotificationSettingNames.ReceiveNotifications, input.ReceiveNotifications.ToString());

            foreach (var notification in input.Notifications)
                if (notification.IsSubscribed)
                    await _notificationSubscriptionManager.SubscribeAsync(AbpSession.ToUserIdentifier(),
                        notification.Name);
                else
                    await _notificationSubscriptionManager.UnsubscribeAsync(AbpSession.ToUserIdentifier(),
                        notification.Name);
        }

        public async Task DeleteNotification(EntityDto<Guid> input)
        {
            var notification = await _userNotificationManager.GetUserNotificationAsync(AbpSession.TenantId, input.Id);
            if (notification.UserId != AbpSession.GetUserId())
                throw new UserFriendlyException(L("ThisNotificationDoesntBelongToYou"));

            await _userNotificationManager.DeleteUserNotificationAsync(AbpSession.TenantId, input.Id);
        }
    }
}