using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.Admin.Web.Host.Files
{
    public class SaveIconInput
    {
        public int Width { get; set; } = 32;

        public int Height { get; set; } = 32;

        public string FileName { get; set; }
    }
}
