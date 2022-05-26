﻿// ======================================================================
// 
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司
//           All rights reserved
// 
//           filename : TwoFactorLoginSettingsEditDto.cs
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

namespace Magicodes.Admin.Application.Configuration.Host.Dto
{
    public class TwoFactorLoginSettingsEditDto
    {
        public bool IsEnabledForApplication { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsEmailProviderEnabled { get; set; }

        public bool IsSmsProviderEnabled { get; set; }

        public bool IsRememberBrowserEnabled { get; set; }

        public bool IsGoogleAuthenticatorEnabled { get; set; }
    }
}