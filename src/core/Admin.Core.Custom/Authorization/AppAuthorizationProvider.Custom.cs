using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using Magicodes.Admin.Localization;

namespace Magicodes.Admin.Core.Custom.Authorization
{
    /// <summary>
    ///     Application's authorization provider.
    ///     Defines permissions for the application.
    ///     See <see cref="AppCustomPermissions" /> for all permission names.
    /// </summary>
    public class AppCustomAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppCustomAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppCustomAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //TODO：用户自定义
            var customs = context.GetPermissionOrNull(AppCustomPermissions.Customs) ?? context.CreatePermission(AppCustomPermissions.Customs, L("Custom"));
            #region WxUser 微信用户
            var wxUser = customs.CreateChildPermission(AppCustomPermissions.Customs_WxUser, L("WxUser"),multiTenancySides:MultiTenancySides.Host);
            wxUser.CreateChildPermission(AppCustomPermissions.Customs_WxUser_Create, L("CreateNew"),multiTenancySides: MultiTenancySides.Host);
            wxUser.CreateChildPermission(AppCustomPermissions.Customs_WxUser_Edit, L("Edit"), multiTenancySides: MultiTenancySides.Host);
            wxUser.CreateChildPermission(AppCustomPermissions.Customs_WxUser_Delete, L("Delete"), multiTenancySides: MultiTenancySides.Host);
            wxUser.CreateChildPermission(AppCustomPermissions.Customs_WxUser_Export, L("ExportToExcel"), multiTenancySides: MultiTenancySides.Host);
            #endregion

            #region MoYuRecord 摸鱼记录
            var moYuRecord = customs.CreateChildPermission(AppCustomPermissions.Customs_MoYuRecord, L("MoYuRecord"), multiTenancySides: MultiTenancySides.Host);
            moYuRecord.CreateChildPermission(AppCustomPermissions.Customs_MoYuRecord_Create, L("CreateNew"), multiTenancySides: MultiTenancySides.Host);
            moYuRecord.CreateChildPermission(AppCustomPermissions.Customs_MoYuRecord_Edit, L("Edit"), multiTenancySides: MultiTenancySides.Host);
            moYuRecord.CreateChildPermission(AppCustomPermissions.Customs_MoYuRecord_Delete, L("Delete"), multiTenancySides: MultiTenancySides.Host);
            moYuRecord.CreateChildPermission(AppCustomPermissions.Customs_MoYuRecord_Export, L("ExportToExcel"), multiTenancySides: MultiTenancySides.Host);
            #endregion
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LocalizationConsts.LocalizationSourceName);
        }
    }
}