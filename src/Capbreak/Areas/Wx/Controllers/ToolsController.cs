using Capbreak.Areas.Wx.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Capbreak.Areas.Wx.Controllers
{
    public class ToolsController : Controller
    {
        private static readonly ConcurrentDictionary<DateTime, MnCastModel> AfdCache = new ConcurrentDictionary<DateTime, MnCastModel>();
        private const int CacheExpirationMinutes = 30;

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

        public ActionResult Radar()
        {
            return View();
        }

        public async Task<ActionResult> MnCast()
        {
            const string MpxAfdEndpoint = "http://kamala.cod.edu/mn/latest.fxus63.KMPX.html";
            const string CoDHrrrEndpoint = "http://weather.cod.edu/forecast/menus/hrrr_menu.php?type=current-FLT-null-null-0-0";
            const string CoDHrrrImageBase = "http://climate.cod.edu/data/forecast/HRRR/{0}/SL/hrrrSL_sfc_radar_{1}.gif";
            var mncast = new MnCastModel { Afd = string.Empty, CodUrls = new List<string>() };

            if (AfdCache.IsEmpty || AfdCache.First().Key < DateTime.UtcNow)
            {
                try
                {
                    AfdCache.Clear();
                    var response = string.Empty;

                    // CoD HRRR
                    using (var client = new HttpClient())
                    {
                        response = await client.GetStringAsync(CoDHrrrEndpoint);
                    }

                    if (!String.IsNullOrEmpty(response))
                    {
                        var document = new HtmlAgilityPack.HtmlDocument();
                        document.LoadHtml(response);
                        if (document.DocumentNode != null)
                        {
                            var anchor = document.DocumentNode.SelectNodes("//a")
                                .Where(x => x.Attributes.Contains("class")
                                    && x.Attributes["class"].Value.Contains("Selected")).FirstOrDefault();

                            if (anchor != null)
                            {
                                var hour = Int32.Parse(anchor.InnerText.Replace("Z", string.Empty).Replace("*", string.Empty));
                                hour = (hour > 0) ? (hour - 1) : 23;    // Normally I'd check for inProgress class, but sometimes "completed" has blank images. Let's just always use the previous one.
                                var realHour = (hour < 10) ? String.Format("0{0}", hour) : hour.ToString();

                                for (var i = 0; i < 15; i++)
                                {
                                    var adjHour = (i < 10) ? String.Format("00{0}", i) : String.Format("0{0}", i);
                                    mncast.CodUrls.Add(String.Format(CoDHrrrImageBase, realHour, adjHour));
                                }
                            }
                        }
                    }

                    // Kamala AFD text
                    using (var client = new HttpClient())
                    {
                        response = await client.GetStringAsync(MpxAfdEndpoint);
                    }

                    if (!String.IsNullOrEmpty(response))
                    {
                        var document = new HtmlAgilityPack.HtmlDocument();
                        document.LoadHtml(response);
                        if (document.DocumentNode != null)
                        {
                            var prenode = document.DocumentNode.SelectSingleNode("//pre");
                            if (prenode != null)
                            {
                                mncast.Afd = prenode.InnerHtml;
                                mncast.Afd = mncast.Afd.Replace("'", "&quot;");
                            }
                        }
                    }

                    AfdCache.GetOrAdd(DateTime.UtcNow.AddMinutes(CacheExpirationMinutes), mncast);
                }
                catch (Exception ex)
                {
                    // TODO
                }
            }
            else
            {
                mncast = AfdCache.First().Value;
            }

            return View("MnCast", mncast);
        }
	}
}