using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Capbreak.Protocol.Models;
using System.Xml;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

namespace Capbreak.Protocol.NexradSiteList
{
    public class NexradSiteListService
    {
        public async Task<List<NexradSite>> GetSites()
        {
            // https://explore.data.gov/Geography-and-Environment/Next-Generation-Radar-NEXRAD-Locations/aa5v-8afd
            var endpoint = "http://www.ncdc.noaa.gov/oa/radar/nexrad.kmz";
            Stream ms = new MemoryStream();
            var sitelist = new List<NexradSite>();

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStreamAsync(endpoint);
                    if (response != null)
                        response.CopyTo(ms);
                }

                if (ms != null && ms.Length > 0)
                {
                    ms.Position = 0;

                    // Doing it by hand
                    var zip = new ZipArchive(ms);
                    var entry = zip.Entries.FirstOrDefault(x => x.FullName.EndsWith(".kml", StringComparison.OrdinalIgnoreCase));
                    if (entry != null)
                    {
                        var ks = entry.Open();
                        var kml = new XmlDocument();
                        kml.Load(ks);
                        var xmlnsManager = new System.Xml.XmlNamespaceManager(kml.NameTable);
                        xmlnsManager.AddNamespace("k", "http://earth.google.com/kml/2.0");
                        var wsrnodes = kml.SelectNodes("//k:wsr", xmlnsManager);
                        foreach (XmlNode wsr in wsrnodes)
                        {
                            var name = wsr.SelectSingleNode("k:name", xmlnsManager).InnerText;
                            var point = wsr.SelectSingleNode("//k:coordinates", xmlnsManager).InnerText.Split(',');
                            var lat = float.Parse(point[0]);
                            var lon = float.Parse(point[1]);
                            var description = wsr.SelectSingleNode("k:description", xmlnsManager).InnerText;

                            sitelist.Add(new NexradSite
                            {
                                
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var meow = ex.ToString();
                // TODO logging
            }

            return null;
        }
    }
}
