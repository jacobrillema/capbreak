﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Capbreak.Protocol.NexradParser;
using Capbreak.Protocol.NexradSiteList;
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