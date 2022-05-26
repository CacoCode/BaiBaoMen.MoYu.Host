using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.Admin.Core
{
    /// <summary>
    /// 通用验证常量定义，以便统一大部分字段精度
    /// </summary>
    // ReSharper disable once IdentifierTypo
    public class ValidationConsts
    {
        /// <summary>
        /// 名称最大长度
        /// </summary>
        public const int NameMaxLength = 64;

        /// <summary>
        /// 手机号码最大长度
        /// </summary>
        public const int PhoneNumberMaxLength = 15;

        /// <summary>
        /// 备注最大长度
        /// </summary>
        public const int RemarkMaxLength = 500;

        /// <summary>
        /// 身份证最大长度
        /// </summary>
        public const int IdCardMaxLength = 18;

        /// <summary>
        /// 范围路径最大长度
        /// </summary>
        public const int PathMaxLength = 256;
    }
}
