using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Capbreak.Protocol.Warnings
{
    public class WarningsService
    {
        private readonly MemoryCache cache = MemoryCache.Default;
        private readonly CacheItemPolicy policy = new CacheItemPolicy();

        // http://warnings.cod.edu/warnings_20140425_00.txt GR uses this
        // http://alerts.weather.gov/cap/us.php?x=1
        // http://alerts.weather.gov/cap/us.atom

        public async Task<feed> FetchWarnings()
        {
            var endpoint = "http://alerts.weather.gov/cap/us.atom";
            var warningsfeed = new feed();

            // Check the cache
            warningsfeed = cache.Get("warnings") as feed;
            if (warningsfeed != null)
                return warningsfeed;

            // Cache not found - download, deserialize, and cache
            Stream response;
            using (var client = new HttpClient())
            {
                response = await client.GetStreamAsync(endpoint);
            }

            if (response != null)
            {
                var stream = new MemoryStream();
                response.CopyTo(stream);
                stream.Position = 0;
                var ser = new XmlSerializer(typeof(feed));
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    warningsfeed = (feed)ser.Deserialize(reader);
                }

                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1);
                cache.Add("warnings", warningsfeed, policy);
            }

            return warningsfeed;
        }
    }
}
