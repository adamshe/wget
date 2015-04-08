using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models.DataPoint;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.Core.Web.Extensions;
using System.Xml.Linq;
namespace NB.Core.Web.DownloadClient
{
    public class SecEdgarDownloader : BaseDownloader<PositionChangeDataPointAggregate>
    {
        public SecEdgarDownloader(SecEdgarSetting setting)
            : base(setting)
        {
        }

        protected override PositionChangeDataPointAggregate ConvertResult(string contentStr, string ticker = "")
        {
            string startPattern = "<table class=\"tableFile\" summary=\"Document Format Files\">";
            string endPattern = "</table>";
            int startIndex = contentStr.IndexOfOccurence(startPattern, 1);
            int endIndex = contentStr.IndexOf(endPattern, startIndex);
            string normalizeContent = contentStr.Substring(startIndex, endIndex - startIndex + endPattern.Length);
            XParseDocument doc = MyHelper.ParseXmlDocument(normalizeContent);

            XParseElement resultNode = XPath.GetElement("//table/tr[3]/td[3]/a", doc);

            var xmlLink = resultNode.Attribute("href").Value;
            var xmlResult = ParseForm4(GetSecPath(xmlLink));
            var aggregate = new PositionChangeDataPointAggregate(xmlResult);
            aggregate.PopulatePoints();
            Console.WriteLine(xmlResult);
            ////*[@id="formDiv"]/div/table/tbody/tr[3]/td[3]/a
            return aggregate;
        }

        private string GetSecPath (string xmllink)
        {
            return "http://www.sec.gov/" + xmllink;
        }

        public static XDocument ParseForm4(string uri)
        {
            if (!string.IsNullOrEmpty(uri))
            {
                var src = XDocument.Load(uri);
                var ticker = src.Descendants("issuer").First().Element("issuerTradingSymbol").Value;
                var doc = new XDocument(new XElement(ticker));
                var element = from el in src.Root.Descendants("nonDerivativeTransaction")
                              // where el.Descendants("transactionAmounts").First().Element("transactionAcquiredDisposedCode").Element("value").Value == "A"
                              select new XElement("Entry",
                                     new XAttribute("Date", el.Descendants("transactionDate").First().Element("value").Value),
                                     new XAttribute("Share", el.Descendants("transactionAmounts").First().Element("transactionShares").Element("value").Value),
                                     new XAttribute("Price", el.Descendants("transactionAmounts").First().Element("transactionPricePerShare").Element("value").Value),
                                     new XAttribute("AquiredDisposeCode", el.Descendants("transactionAmounts").First().Element("transactionAcquiredDisposedCode").Element("value").Value)
                                     );
                doc.Root.Add(element);
                return doc;
            }
            return null;
        }
    }
}
