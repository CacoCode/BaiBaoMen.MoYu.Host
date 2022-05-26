using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Magicodes.Admin.Core.Custom.WxUsers;

namespace Magicodes.Admin.Application.Custom.WxUsers.Dto
{
    [AutoMapFrom(typeof(WxUser))]
    public class UpdateWxUserInput : EntityDto<long>
    {
    }
}
