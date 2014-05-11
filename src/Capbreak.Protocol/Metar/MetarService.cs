using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Capbreak.Protocol.Models;
using Capbreak.Core;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Capbreak.Protocol.Metar
{
    public interface IMetarService : IService
    {
        Task<IemMetarResponse> FetchMetars();
    }

    public class MetarService
    {
        // http://aviationweather.gov/static/adds/metars/stations.txt
        // http://aviationweather.gov/gis/scripts/MetarJSON.php
        // http://mesonet.agron.iastate.edu/geojson/network_obs.php?network=MN_ASOS
        // http://www.aviationweather.gov/adds/dataserver/metars/MetarsXMLOutput.php
        // http://www.aviationweather.gov/adds/dataserver_current/httpparam?dataSource=metars&requestType=retrieve&format=xml&stationString=@OK&hoursBeforeNow=1&mostRecentForEachStation=constraint&fields=station_id,latitude,longitude,temp_c,dewpoint_c,wind_dir_degrees,wind_speed_kt,wind_gust_kt,sky_cover,sea_level_pressure_mb,wx_string,observation_time,altim_in_hg,raw_text

        private readonly MemoryCache cache = MemoryCache.Default;
        private readonly CacheItemPolicy policy = new CacheItemPolicy();

        public async Task<AddsMetarResponse> FetchMetars(string state, string hash)
        {
            const string addsUrl = "http://www.aviationweather.gov/adds/dataserver_current/httpparam?dataSource=metars&format=xml&requestType=retrieve&stationString=@{0}&hoursBeforeNow=1&mostRecentForEachStation=constraint&fields=station_id,latitude,longitude,temp_c,dewpoint_c,wind_dir_degrees,wind_speed_kt,wind_gust_kt,sky_cover,sea_level_pressure_mb,wx_string,observation_time";
            var metarResponse = new AddsMetarResponse { NewData = true };

            try
            {
                state = state.ToUpperInvariant();
                var key = String.Format("metar|{0}", state);

                // Check cache and return METARs if found - if the hash matches, let the client know there isn't new data to download
                metarResponse = cache.Get(key) as AddsMetarResponse;
                if (metarResponse != null)
                {
                    if (!String.IsNullOrEmpty(hash) && hash.Equals(metarResponse.Hash))
                        return new AddsMetarResponse() { Hash = hash, State = state, NewData = false };

                    return metarResponse;
                }

                // Cache not found - download, deserialize, and cache
                var endpoint = String.Format(addsUrl, state);
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
                    var ser = new XmlSerializer(typeof(AddsMetarResponse));
                    using (XmlReader reader = XmlReader.Create(stream))
                    {
                        metarResponse = (AddsMetarResponse)ser.Deserialize(reader);
                    }

                    var newhash = metarResponse.GetHashCode().ToString();
                    metarResponse.State = state;
                    metarResponse.Hash = newhash;
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5);
                    cache.Add(key, metarResponse, policy);
                }
            }
            catch (Exception ex)
            {
                // TODO add logging
            }

            return metarResponse;
        }
    }
}