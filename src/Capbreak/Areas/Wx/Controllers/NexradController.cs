using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Capbreak.Protocol.NexradParser;
using System.Web.Mvc;
using System.Threading.Tasks;
using Capbreak.Protocol.Models;
using Newtonsoft.Json;

namespace Capbreak.Areas.Wx.Controllers
{
    // TODO use Web API
    public class NexradController : Controller
    {
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

        public async Task<string> Latest(string site, string product)
        {
            site = site.ToUpperInvariant();
            product = product.ToUpperInvariant();
            var filelist = await List(site, product);
            var latestfile = filelist.Last();
            var latestscan = await Scan(latestfile, site, product);
            var latestscanjson = JsonConvert.SerializeObject(latestscan);

            return latestscanjson;
        }
    }
}