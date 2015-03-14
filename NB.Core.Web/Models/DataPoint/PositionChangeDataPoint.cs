using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NB.Core.Web.Models.DataPoint
{

    public class PositionChangeDataPointAggregate
    {
        XDocument _doc;
        PositionChangeDataPoint[] _item;
        public PositionChangeDataPointAggregate(XDocument doc)
        {
            _doc = doc;
        }
        public PositionChangeDataPointAggregate(PositionChangeDataPoint[] items)
        {
            Items = items;
        }
        public PositionChangeDataPoint[] Items 
        { 
            get { return _item; } 
            set { _item = value; } 
        }

        public void PopulatePoints ()
        {
            PopulatePoints(_doc);
        }

        public override string ToString()
        {
            return _doc.ToString();
        }
        public void PopulatePoints (XDocument doc)
        {
            var list = new List<PositionChangeDataPoint>();
            var element = from el in doc.Root.Descendants("Entry")
                             // where el.Descendants("transactionAmounts").First().Element("transactionAcquiredDisposedCode").Element("value").Value == "A"
                              select new PositionChangeDataPoint{ 
                                     Date = DateTime.Parse(el.Attribute("Date").Value),
                                     Qantity = ShareSign(el.Attribute("AquiredDisposeCode").Value) *
                                     int.Parse(el.Attribute("Share").Value),
                                     Price = float.Parse(el.Attribute("Price").Value),
                                     Ticker = doc.Root.Name.LocalName
                                     };
            list.AddRange(element);
            _item = list.ToArray();
        }

        private int ShareSign (string acquireOrDispose)
        {
            if (string.Compare(acquireOrDispose, "Q", false) == 0)
                return -1;
            return 1;
        }
    }
    public class PositionChangeDataPoint
    {
        public DateTime Date { get; set; }

        public int Qantity { get; set; }

        public float Price { get; set; }

        public string Ticker { get; set; }

        public string TickerCIK { get; set; }

        public string InstitutionName { get; set; }

        public string CIK { get; set; }
    }
}
