﻿// ======================================================================
// 
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司
//           All rights reserved
// 
//           filename : AdminDbContext.Custom.cs
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

using Magicodes.Admin.Core.Custom.MoYuRecords;
using Magicodes.Admin.Core.Custom.WxUsers;
using Microsoft.EntityFrameworkCore;

namespace Magicodes.Admin.EntityFrameworkCore.EntityFramework
{
    public partial class AdminDbContext
    {
        //TODO:用户自定义
        public virtual DbSet<WxUser> WxUsers { get; set; }

        public virtual DbSet<MoYuRecord> MoYuRecords { get; set; }
    }
}