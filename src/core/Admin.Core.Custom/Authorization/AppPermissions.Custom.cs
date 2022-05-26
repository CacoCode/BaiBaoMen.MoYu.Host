namespace Magicodes.Admin.Core.Custom.Authorization
{
    /// <summary>
    ///     定义应用程序权限名称常量
    ///     <see cref="Core.Custom.Authorization.AppCustomAuthorizationProvider" /> 权限定义.
    /// </summary>
    public class AppCustomPermissions
    {
        //TODO:用户自定义

        /// <summary>
        /// 自定义
        /// </summary>
        public const string Customs = "Customs";

        #region WxUser 微信用户
        public const string Customs_WxUser = "Customs.WxUser";
        public const string Customs_WxUser_Create = "Customs.WxUser.Create";
        public const string Customs_WxUser_Edit = "Customs.WxUser.Edit";
        public const string Customs_WxUser_Delete = "Customs.WxUser.Delete";
        public const string Customs_WxUser_Export = "Customs.WxUser.Export";

        #endregion

        #region MoYuRecord 摸鱼记录
        public const string Customs_MoYuRecord = "Customs.MoYuRecord";
        public const string Customs_MoYuRecord_Create = "Customs.MoYuRecord.Create";
        public const string Customs_MoYuRecord_Edit = "Customs.MoYuRecord.Edit";
        public const string Customs_MoYuRecord_Delete = "Customs.MoYuRecord.Delete";
        public const string Customs_MoYuRecord_Export = "Customs.MoYuRecord.Export";

        #endregion
    }
}