using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capbreak.Protocol.Warnings
{
    public class PolygonProduct
    {
        public ProductCode Code { get; set; }
        public List<PointF> Points { get; set; }
    }

    public enum ProductCode
    {
        SevereThunderStorm,
        Tornado
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public partial class feed
    {
        private string idField;
        private string logoField;
        private string generatorField;
        private System.DateTime updatedField;
        private feedAuthor authorField;
        private string titleField;
        private feedLink linkField;
        private feedEntry[] entryField;

        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        public string logo
        {
            get
            {
                return this.logoField;
            }
            set
            {
                this.logoField = value;
            }
        }

        public string generator
        {
            get
            {
                return this.generatorField;
            }
            set
            {
                this.generatorField = value;
            }
        }

        public System.DateTime updated
        {
            get
            {
                return this.updatedField;
            }
            set
            {
                this.updatedField = value;
            }
        }

        public feedAuthor author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        public feedLink link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("entry")]
        public feedEntry[] entry
        {
            get
            {
                return this.entryField;
            }
            set
            {
                this.entryField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class feedAuthor
    {
        private string nameField;
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

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class feedLink
    {
        private string hrefField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class feedEntry
    {
        private string idField;
        private System.DateTime updatedField;
        private System.DateTime publishedField;
        private feedEntryAuthor authorField;
        private string titleField;
        private feedEntryLink linkField;
        private string summaryField;
        private string eventField;
        private System.DateTime effectiveField;
        private System.DateTime expiresField;
        private string statusField;
        private string msgTypeField;
        private string categoryField;
        private string urgencyField;
        private string severityField;
        private string certaintyField;
        private string areaDescField;
        private string polygonField;
        private geocode geocodeField;
        private parameter parameterField;

        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        public System.DateTime updated
        {
            get
            {
                return this.updatedField;
            }
            set
            {
                this.updatedField = value;
            }
        }

        public System.DateTime published
        {
            get
            {
                return this.publishedField;
            }
            set
            {
                this.publishedField = value;
            }
        }

        public feedEntryAuthor author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        public feedEntryLink link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        public string summary
        {
            get
            {
                return this.summaryField;
            }
            set
            {
                this.summaryField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public string @event
        {
            get
            {
                return this.eventField;
            }
            set
            {
                this.eventField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public System.DateTime effective
        {
            get
            {
                return this.effectiveField;
            }
            set
            {
                this.effectiveField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public System.DateTime expires
        {
            get
            {
                return this.expiresField;
            }
            set
            {
                this.expiresField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public string msgType
        {
            get
            {
                return this.msgTypeField;
            }
            set
            {
                this.msgTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public string category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public string urgency
        {
            get
            {
                return this.urgencyField;
            }
            set
            {
                this.urgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public string severity
        {
            get
            {
                return this.severityField;
            }
            set
            {
                this.severityField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public string certainty
        {
            get
            {
                return this.certaintyField;
            }
            set
            {
                this.certaintyField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public string areaDesc
        {
            get
            {
                return this.areaDescField;
            }
            set
            {
                this.areaDescField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public string polygon
        {
            get
            {
                return this.polygonField;
            }
            set
            {
                this.polygonField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public geocode geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
        public parameter parameter
        {
            get
            {
                return this.parameterField;
            }
            set
            {
                this.parameterField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class feedEntryAuthor
    {
        private string nameField;

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

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class feedEntryLink
    {
        private string hrefField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1", IsNullable = false)]
    public partial class geocode
    {
        private string[] itemsField;
        private ItemsChoiceType[] itemsElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("value", typeof(string), Namespace = "http://www.w3.org/2005/Atom")]
        [System.Xml.Serialization.XmlElementAttribute("valueName", typeof(string), Namespace = "http://www.w3.org/2005/Atom")]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public string[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {
        [System.Xml.Serialization.XmlEnumAttribute("http://www.w3.org/2005/Atom:value")]
        value,

        [System.Xml.Serialization.XmlEnumAttribute("http://www.w3.org/2005/Atom:valueName")]
        valueName,
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:emergency:cap:1.1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:emergency:cap:1.1", IsNullable = false)]
    public partial class parameter
    {
        private string valueNameField;
        private string valueField;

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom")]
        public string valueName
        {
            get
            {
                return this.valueNameField;
            }
            set
            {
                this.valueNameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom")]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}