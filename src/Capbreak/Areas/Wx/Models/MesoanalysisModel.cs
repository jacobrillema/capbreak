using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capbreak.Areas.Projects.Models
{
    // TODO this isn't even used
    public class MesoanalysisModel
    {
        public string Title { get; set; }
        public string RefreshSeconds { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Data { get; set; }
    }
}