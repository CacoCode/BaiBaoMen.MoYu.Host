using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Magicodes.Admin.Unity.Extension
{
    public static class EnumOperate
    {

        /// <summary>        
        /// 扩展方法，获得枚举的Description
        ///</summary>        
        ///<param name="value">枚举值</param>        
        ///<param name="nameInstead">当枚举值没有定义DisplayNameAttribute，是否使用枚举名代替，默认是使用</param>        
        ///<returns>枚举的DisplayName</returns>        
        public static string GetDisplayName(this Enum value, Boolean nameInstead = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            DisplayAttribute attribute = Attribute.GetCustomAttribute(type.GetField(name), typeof(DisplayAttribute)) as DisplayAttribute;
            if (attribute == null && nameInstead == true)
            {
                return name;
            }

            return attribute == null ? null : attribute.Name;
        }

    }
}
