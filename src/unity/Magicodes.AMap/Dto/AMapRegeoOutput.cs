using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.AMap.Dto
{
    public class AMapRegeoOutput
    {
        public string Status { get; set; }

        public string Info { get; set; }

        public string Infocode { get; set; }

        public RegeoCode Regeocode { get; set; }

        public List<RegeoCode> Regeocodes { get; set; }
    }

    public class RegeoCode
    {
        public AddressComponent AddressComponent { get; set; }

        public string Formatted_address { get; set; }
    }

    public class AddressComponent
    {
        public string Province { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Adcode { get; set; }
    }
}
