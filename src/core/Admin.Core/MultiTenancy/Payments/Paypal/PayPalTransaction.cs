// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : PayPalTransaction.cs
//           description :
// 
//           created by ѩ�� at  2019-06-17 10:17
//           �����ĵ�: docs.xin-lai.com
//           ���ںŽ̳̣�magiccodes
//           QQȺ��85318032����̽�����
//           Blog��http://www.cnblogs.com/codelove/
//           Home��http://xin-lai.com
// 
// ======================================================================

using Newtonsoft.Json;

namespace Magicodes.Admin.Core.MultiTenancy.Payments.Paypal
{
    public class PayPalTransaction
    {
        [JsonProperty("amount")] public PayPalAmount Amount { get; set; }

        [JsonProperty("description")] public string Description { get; set; }
    }
}