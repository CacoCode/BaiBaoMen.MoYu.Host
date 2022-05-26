using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.Admin.Application.Core.Dto
{
    public class LocationInfoDto
    {
        /// <summary>
        /// 设备标识
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
