// ======================================================================
// 
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司
//           All rights reserved
// 
//           filename : AdminTestModule.cs
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

using Abp.Modules;
using Abp.TestBase;
using Magicodes.Admin.Application;
using Magicodes.Admin.Application.Custom;
using Magicodes.Admin.EntityFrameworkCore.EntityFramework;
using Magicodes.Admin.Tests.Base;

namespace Magicodes.Admin.Tests
{
    [DependsOn(typeof(AdminTestBaseModule))]
    public class AdminTestModule : AbpModule
    {

    }
}