﻿// ======================================================================
// 
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司
//           All rights reserved
// 
//           filename : GetRolesInput.cs
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

using System.Collections.Generic;
using Abp.Runtime.Validation;
using Magicodes.Admin.Application.Core.Dto;

namespace Magicodes.Admin.Application.Authorization.Roles.Dto
{
    public class GetRolesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public List<string> PermissionNames { get; set; }

        public string FilterText { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting)) Sorting = "Id";
        }
    }
}