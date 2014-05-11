using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Capbreak.Protocol.Models
{
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "response")]
    public class AddsMetarResponse
    {
        private uint request_indexField;
        private responseData_source data_sourceField;
        private responseRequest requestField;
        private object errorsField;
        private responseWarnings warningsField;
        private ushort time_taken_msField;
        private responseData dataField;
        private decimal versionField;
        private string noNamespaceSchemaLocationField;

        public string Hash { get; set; }
        public bool NewData { get; set; }
        public string State { get; set; }

        [JsonIgnore]
        public uint request_index
        {
            get
            {
                return this.request_indexField;
            }
            set
            {
                this.request_indexField = value;
            }
        }

        [JsonIgnore]
        public responseData_source data_source
        {
            get
            {
                return this.data_sourceField;
            }
            set
            {
                this.data_sourceField = value;
            }
        }

        [JsonIgnore]
        public responseRequest request
        {
            get
            {
                return this.requestField;
            }
            set
            {
                this.requestField = value;
            }
        }

        [JsonIgnore]
        public object errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
            }
        }

        [JsonIgnore]
        public responseWarnings warnings
        {
            get
            {
                return this.warningsField;
            }
            set
            {
                this.warningsField = value;
            }
        }

        [JsonIgnore]
        public ushort time_taken_ms
        {
            get
            {
                return this.time_taken_msField;
            }
            set
            {
                this.time_taken_msField = value;
            }
        }

        public responseData data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }

        [JsonIgnore]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [JsonIgnore]
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/2001/XML-Schema-instance")]
        public string noNamespaceSchemaLocation
        {
            get
            {
                return this.noNamespaceSchemaLocationField;
            }
            set
            {
                this.noNamespaceSchemaLocationField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class responseData_source
    {
        private string nameField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class responseRequest
    {
        private string typeField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class responseWarnings
    {
        private string warningField;

        public string warning
        {
            get
            {
                return this.warningField;
            }
            set
            {
                this.warningField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class responseData
    {
        private responseDataMETAR[] mETARField;
        private byte num_resultsField;

        [System.Xml.Serialization.XmlElementAttribute("METAR")]
        public responseDataMETAR[] METAR
        {
            get
            {
                return this.mETARField;
            }
            set
            {
                this.mETARField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte num_results
        {
            get
            {
                return this.num_resultsField;
            }
            set
            {
                this.num_resultsField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class responseDataMETAR
    {
        private string station_idField;
        private DateTime observation_timeField;
        private decimal latitudeField;
        private decimal longitudeField;
        private decimal temp_cField;
        private decimal dewpoint_cField;
        private int wind_dir_degreesField;
        private int wind_speed_ktField;
        // TODO sky cover

        [System.Xml.Serialization.XmlElementAttribute("station_id")]
        public string id
        {
            get
            {
                return this.station_idField;
            }
            set
            {
                this.station_idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("observation_time")]
        public DateTime time
        {
            get
            {
                return this.observation_timeField;
            }
            set
            {
                this.observation_timeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("latitude")]
        public decimal lat
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("longitude")]
        public decimal lon
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("temp_c")]
        public decimal temp
        {
            get
            {
                return this.temp_cField;
            }
            set
            {
                this.temp_cField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("dewpoint_c")]
        public decimal dewp
        {
            get
            {
                return this.dewpoint_cField;
            }
            set
            {
                this.dewpoint_cField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("wind_dir_degrees")]
        public int wdir
        {
            get
            {
                return this.wind_dir_degreesField;
            }
            set
            {
                this.wind_dir_degreesField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("wind_speed_kt")]
        public int wspd
        {
            get
            {
                return this.wind_speed_ktField;
            }
            set
            {
                this.wind_speed_ktField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class responseDataMETARSky_condition
    {
        private string sky_coverField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sky_cover
        {
            get
            {
                return this.sky_coverField;
            }
            set
            {
                this.sky_coverField = value;
            }
        }
    }
}