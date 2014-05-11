using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Web.Http;
using Capbreak.Protocol.NexradParser;
using Capbreak.Protocol.NexradSiteList;
using Capbreak.Protocol.Warnings;
using Capbreak.Protocol.Metar;
using System.Web.Mvc;
using System.Threading.Tasks;
using Capbreak.Protocol.Models;
using Newtonsoft.Json;
using Capbreak.Areas.Wx.Helpers;
using System.Drawing;
using ICSharpCode.SharpZipLib.BZip2;
using System.Text;

namespace Capbreak.Areas.Wx.Controllers
{
    // TODO use Web API
    public class WarningResponse
    {
        public string ProductCode { get; set; }
        public List<List<PointF>> Polygons { get; set; }
    }

    public class NexradController : Controller
    {
        public ActionResult TestBzip()
        {
            try
            {
                const string test = "hello world";
                Stream inS = new MemoryStream(Encoding.ASCII.GetBytes(test ?? ""));
                Stream outS = new MemoryStream();
                BZip2.Compress(inS, outS, false, 9);
                outS.Position = 0;
                var reader = new StreamReader(outS);
                var text = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                var meow = ex;
            }

            return null;
        }

        public async Task<string> WarningsFeed(string type)
        {
            var warningService = new WarningsService();
            var response = await warningService.FetchWarnings();

            if (String.IsNullOrEmpty(type))
                return JsonConvert.SerializeObject(response);

            // User passed a type, time to filter
            // TODO unrecognized types
            var warningResponses = new List<WarningResponse>();
            var codes = type.Split('|');
            foreach (var code in codes)
            {
                var atomCode = code;
                switch (code.ToLowerInvariant())
                {
                    case "tor":
                        atomCode = "Tornado Warning";
                        break;
                }

                var rawProducts = response.entry.Where(x => x.@event == atomCode);
                var polygons = new List<List<PointF>>();

                foreach (var product in rawProducts)
                {
                    var polygon = NexradHelpers.ConvertStringToPolygon(product.polygon);
                    polygons.Add(polygon);
                }

                warningResponses.Add(new WarningResponse() { ProductCode = code, Polygons = polygons });
            }

            return JsonConvert.SerializeObject(warningResponses);
        }

        public async Task<string> MetarFeed(string state, string hash)
        {
            const bool UseBzipCompression = true;
            var metarservice = new MetarService();
            var metars = await metarservice.FetchMetars(state, hash);

            if (UseBzipCompression)
            {

            }

            return JsonConvert.SerializeObject(metars);
        }

        public async Task<List<string>> List(string site, string product)
        {
            // TODO add IoC for services
            var latest = string.Empty;
            var nexradParser = new NexradParserService();
            var filelist = await nexradParser.FetchNexradFileListAsync(site, product);

            return filelist;
        }

        public async Task<NexradScan> Scan(string filename, string site, string product)
        {
            var nexradParser = new NexradParserService();
            var scan = await nexradParser.FetchNexradScanAsync(site, product, filename);

            return scan;
        }

        // TODO handle newer forms of reflectivity
        // TODO handle public vs AH
        public async Task<string> Latest(string site, string product)
        {
            var latestfile = string.Empty;
            //var filelist = await List(site, product);
            //var latestfile = filelist.Last();

            if (String.IsNullOrEmpty(latestfile))
                latestfile = "sn.last";

            var latestscan = await Scan(latestfile, site, product);
            var latestscanjson = JsonConvert.SerializeObject(latestscan);

            return latestscanjson;
        }

        public async Task<string> SiteList()
        {
            var nexradSiteListService = new NexradSiteListService();
            var sitelist = await nexradSiteListService.GetSites();
            var response = JsonConvert.SerializeObject(sitelist);

            return response;
        }
    }
}