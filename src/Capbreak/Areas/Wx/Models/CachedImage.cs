using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capbreak.Areas.Wx.Models
{
    public class CachedImage
    {
        public string Name { get; set; }
        public Byte[] ImageBytes { get; set; }
    }
}