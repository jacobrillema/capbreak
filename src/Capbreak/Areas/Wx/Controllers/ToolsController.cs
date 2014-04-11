using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capbreak.Areas.Wx.Controllers
{
    public class ToolsController : Controller
    {
        //
        // GET: /Wx/Tools/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SpcArchive()
        {
            return View();
        }
	}
}