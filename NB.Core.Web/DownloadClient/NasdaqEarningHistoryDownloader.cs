using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Models.Metadata;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NB.Core.Web.DownloadClient
{
    public class NasdaqEarningHistoryDownloader : BaseDownloader<EarningHistoryDataAggregate>
    {
        public NasdaqEarningHistoryDownloader(BaseSetting setting) : base (setting)
        {

        }

        public NasdaqEarningHistoryDownloader()
            : this(new NasdaqEarningHistorySetting())
        {
        }

        protected override EarningHistoryDataAggregate ConvertResult(string content, string ticker = "")
        {
            List<EarningHistoryData> history = new List<EarningHistoryData>(10);
            var matchPattern = "(<div class=\"genTable\">.*?</div>)";
            var match = Regex.Matches(content, matchPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

            XParseDocument doc = MyHelper.ParseXmlDocument(match[0].Groups[0].Value);

            var resultNode = XPath.GetElement("//table", doc);
            ParseTable(history, resultNode, ticker, "");
            return new EarningHistoryDataAggregate(history.ToArray(), ticker);
        }

        private static void ParseTable(List<EarningHistoryData> earningHistory, XParseElement sourceNode, string symbol, string xPath)
        {
            var resultNode = sourceNode;
            if (!(string.IsNullOrWhiteSpace(xPath) || string.IsNullOrEmpty(xPath)))
                resultNode = XPath.GetElement(xPath, sourceNode);
            int cnt = 0;
            XParseElement targetNode;
            XPathAttribute xpath;
            object value;
            if (resultNode != null)
            {
                foreach (XParseElement node in resultNode.Elements())
                {
                    if (node.Name.LocalName == "tr")
                    {
                        cnt++;
                        if (cnt > 1) // skip row header
                        {
                            var data = new EarningHistoryData();
                            foreach (var property in data.GetType().GetProperties())
                            {
                                xpath = property.GetCustomAttributes(typeof(XPathAttribute), false).FirstOrDefault() as XPathAttribute;
                                if (xpath == null)
                                    continue;
                                targetNode = XPath.GetElement(xpath.Path, node);
                                value = Convert.ChangeType(targetNode.Value, property.PropertyType);
                                property.SetValue(data, value);
                            }

                            earningHistory.Add(data);
                        }
                    }
                }
            }
        }
    }
}
