using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Magicodes.Admin.Core.Custom
{
    /// <summary>
    ///     基础模型
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class CustomEntityBase<TKey> :
        Entity<TKey>,
        IMayHaveTenant
    {
        /// <summary>
        ///     租户Id
        /// </summary>
        [Display(Name = "租户Id")]
        public int? TenantId { get; set; }
    }
}