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

namespace Capbreak.Protocol.NexradSiteList
{
    public class NexradSiteListService
    {
        // TODO add caching
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
                            try
                            {
                                var name = wsr.SelectSingleNode(".//k:name", xmlnsManager).InnerText;
                                var point = wsr.SelectSingleNode(".//k:coordinates", xmlnsManager).InnerText.Split(',');
                                var longitude = float.Parse(point[0]);
                                var latitude = float.Parse(point[1]);
                                var cdataText = ((XmlCDataSection)(wsr.SelectSingleNode(".//k:description", xmlnsManager).ChildNodes[0])).InnerText;
                                var cdataSplit = cdataText.Split(new string[] { "<BR>" }, StringSplitOptions.RemoveEmptyEntries);
                                var location = cdataSplit.FirstOrDefault(x => x.Contains("LOCATION:"));
                                location = location.Replace("LOCATION:", string.Empty).Trim();
                                var elevationString = cdataSplit.FirstOrDefault(x => x.Contains("ELEVATION:"));
                                var elevation = Double.Parse(elevationString.Replace("ELEVATION:", string.Empty).Trim());

                                sitelist.Add(new NexradSite
                                {
                                    Name = name,
                                    Latitude = latitude,
                                    Longitude = longitude,
                                    Elevation = elevation,
                                    Location = location
                                });
                            }
                            catch (Exception ex)
                            {
                                // TODO log errors for individual elements
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO logging
            }

            return sitelist;
        }
    }
}
