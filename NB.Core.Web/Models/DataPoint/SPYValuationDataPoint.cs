using NB.Core.Web.Enums;
using NB.Core.Web.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NB.Core.Web.Models
{
    [Serializable]
    [XmlRoot("Result")]
    public class SPYValuationDataPointAggregate
    {
        public SPYValuationDataPointAggregate()
        {

        }
        public SPYValuationDataPointAggregate(ValuationType type, SPYValuationDataPoint[] items = null, string ticker = "SPY")
        {
            _items = items;
            Ticker = ticker;
            MetricsType = type;
        }

        [XmlAttribute]
        public string Ticker { get; set; }

        [XmlAttribute]
        public ValuationType MetricsType { get; set; }

        private SPYValuationDataPoint[] _items;

        [XmlArray]
        public SPYValuationDataPoint[] Items { get { return _items; } set{_items = value;} }
    }

    
    public class SPYValuationDataPoint
    {        

        [XPath("/td[1]")]
        [XmlElement("date")]
        public DateTime Date { get; set; }

        [XPath("/td[2]")]
        [XmlElement("value")]
        public float Value { get; set; }
    }
}
