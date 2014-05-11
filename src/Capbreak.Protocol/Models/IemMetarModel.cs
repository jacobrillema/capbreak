using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Capbreak.Protocol.Models
{
    public class Properties
    {
        public int code { get; set; }
        public List<int> coordinate_order { get; set; }
    }

    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Properties2
    {
        public string id { get; set; }
        [JsonIgnore]
        public string station { get; set; }
        public string tmpf { get; set; }
        public string dwpf { get; set; }
        public string drct { get; set; }
        public string sknt { get; set; }
        [JsonIgnore]
        public object indoor_tmpf { get; set; }
        [JsonIgnore]
        public string tsf0 { get; set; }
        [JsonIgnore]
        public string tsf1 { get; set; }
        [JsonIgnore]
        public string tsf2 { get; set; }
        [JsonIgnore]
        public string tsf3 { get; set; }
        [JsonIgnore]
        public string rwis_subf { get; set; }
        [JsonIgnore]
        public string scond0 { get; set; }
        [JsonIgnore]
        public string scond1 { get; set; }
        [JsonIgnore]
        public string scond2 { get; set; }
        [JsonIgnore]
        public string scond3 { get; set; }
        [JsonIgnore]
        public string valid { get; set; }
        [JsonIgnore]
        public string pday { get; set; }
        [JsonIgnore]
        public string c1smv { get; set; }
        [JsonIgnore]
        public string c2smv { get; set; }
        [JsonIgnore]
        public string c3smv { get; set; }
        [JsonIgnore]
        public string c4smv { get; set; }
        [JsonIgnore]
        public string c5smv { get; set; }
        [JsonIgnore]
        public string c1tmpf { get; set; }
        [JsonIgnore]
        public string c2tmpf { get; set; }
        [JsonIgnore]
        public string c3tmpf { get; set; }
        [JsonIgnore]
        public string c4tmpf { get; set; }
        [JsonIgnore]
        public string c5tmpf { get; set; }
        public string pres { get; set; }
        [JsonIgnore]
        public int relh { get; set; }
        [JsonIgnore]
        public string srad { get; set; }
        [JsonIgnore]
        public string vsby { get; set; }
        [JsonIgnore]
        public string phour { get; set; }
        [JsonIgnore]
        public string gust { get; set; }
        [JsonIgnore]
        public string raw { get; set; }
        [JsonIgnore]
        public string alti { get; set; }
        [JsonIgnore]
        public object mslp { get; set; }
        [JsonIgnore]
        public object qc_tmpf { get; set; }
        [JsonIgnore]
        public object qc_dwpf { get; set; }
        [JsonIgnore]
        public string rstage { get; set; }
        [JsonIgnore]
        public string ozone { get; set; }
        [JsonIgnore]
        public string co2 { get; set; }
        [JsonIgnore]
        public string pmonth { get; set; }
        [JsonIgnore]
        public string skyc1 { get; set; }
        [JsonIgnore]
        public string skyc2 { get; set; }
        [JsonIgnore]
        public string skyc3 { get; set; }
        [JsonIgnore]
        public string skyl1 { get; set; }
        [JsonIgnore]
        public string skyl2 { get; set; }
        [JsonIgnore]
        public string skyl3 { get; set; }
        [JsonIgnore]
        public string skyc4 { get; set; }
        [JsonIgnore]
        public object skyl4 { get; set; }
        [JsonIgnore]
        public object pcounter { get; set; }
        [JsonIgnore]
        public object discharge { get; set; }
        [JsonIgnore]
        public string iemid { get; set; }
        [JsonIgnore]
        public object p03i { get; set; }
        [JsonIgnore]
        public object p06i { get; set; }
        [JsonIgnore]
        public object p24i { get; set; }
        [JsonIgnore]
        public object max_tmpf_6hr { get; set; }
        [JsonIgnore]
        public object min_tmpf_6hr { get; set; }
        [JsonIgnore]
        public object max_tmpf_24hr { get; set; }
        [JsonIgnore]
        public object min_tmpf_24hr { get; set; }
        [JsonIgnore]
        public string presentwx { get; set; }
        [JsonIgnore]
        public string max_tmpf { get; set; }
        [JsonIgnore]
        public string min_tmpf { get; set; }
        [JsonIgnore]
        public string max_sknt { get; set; }
        [JsonIgnore]
        public string max_gust { get; set; }
        [JsonIgnore]
        public string max_dwpf { get; set; }
        [JsonIgnore]
        public string min_dwpf { get; set; }
        [JsonIgnore]
        public object snow { get; set; }
        [JsonIgnore]
        public object snowd { get; set; }
        [JsonIgnore]
        public object max_tmpf_qc { get; set; }
        [JsonIgnore]
        public object min_tmpf_qc { get; set; }
        [JsonIgnore]
        public object pday_qc { get; set; }
        [JsonIgnore]
        public object snow_qc { get; set; }
        [JsonIgnore]
        public string snoww { get; set; }
        [JsonIgnore]
        public string max_drct { get; set; }
        [JsonIgnore]
        public object max_srad { get; set; }
        [JsonIgnore]
        public object coop_tmpf { get; set; }
        [JsonIgnore]
        public object coop_valid { get; set; }
        [JsonIgnore]
        public object synop { get; set; }
        [JsonIgnore]
        public string name { get; set; }
        [JsonIgnore]
        public string state { get; set; }
        [JsonIgnore]
        public string country { get; set; }
        [JsonIgnore]
        public string elevation { get; set; }
        [JsonIgnore]
        public string network { get; set; }
        [JsonIgnore]
        public string online { get; set; }
        [JsonIgnore]
        public string @params { get; set; }
        [JsonIgnore]
        public string county { get; set; }
        [JsonIgnore]
        public string plot_name { get; set; }
        [JsonIgnore]
        public string climate_site { get; set; }
        [JsonIgnore]
        public object remote_id { get; set; }
        [JsonIgnore]
        public string wfo { get; set; }
        [JsonIgnore]
        public string archive_begin { get; set; }
        [JsonIgnore]
        public object archive_end { get; set; }
        [JsonIgnore]
        public string tzname { get; set; }
        [JsonIgnore]
        public string modified { get; set; }
        [JsonIgnore]
        public string metasite { get; set; }
        [JsonIgnore]
        public object sigstage_low { get; set; }
        [JsonIgnore]
        public object sigstage_action { get; set; }
        [JsonIgnore]
        public object sigstage_bankfull { get; set; }
        [JsonIgnore]
        public object sigstage_flood { get; set; }
        [JsonIgnore]
        public object sigstage_moderate { get; set; }
        [JsonIgnore]
        public object sigstage_major { get; set; }
        [JsonIgnore]
        public object sigstage_record { get; set; }
        [JsonIgnore]
        public string ugc_county { get; set; }
        [JsonIgnore]
        public string ugc_zone { get; set; }
        [JsonIgnore]
        public string ob_pday { get; set; }
        [JsonIgnore]
        public string x { get; set; }
        [JsonIgnore]
        public string y { get; set; }
        [JsonIgnore]
        public string lvalid { get; set; }
        [JsonIgnore]
        public string lmax_gust_ts { get; set; }
        [JsonIgnore]
        public string lmax_sknt_ts { get; set; }
        [JsonIgnore]
        public string sname { get; set; }
        [JsonIgnore]
        public int ts { get; set; }
        [JsonIgnore]
        public object gust_ts { get; set; }
        [JsonIgnore]
        public object lgust_ts { get; set; }
        public string obtime { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        [JsonIgnore]
        public double sped { get; set; }
        [JsonIgnore]
        public object feel { get; set; }
        [JsonIgnore]
        public string peak { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<string> coordinates { get; set; }
    }

    public class Feature
    {
        [JsonIgnore]
        public string type { get; set; }
        
        [JsonIgnore]
        public string id { get; set; }
        
        public Properties2 properties { get; set; }
        
        [JsonIgnore]
        public Geometry geometry { get; set; }
    }

    public class IemMetarResponse
    {
        [JsonIgnore]
        public string type { get; set; }

        [JsonIgnore]
        public Crs crs { get; set; }
        
        public List<Feature> features { get; set; }
        
        public string Hash { get; set; }
        public string State { get; set; }
        public bool NewData { get; set; }
    }
}