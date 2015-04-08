using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Models.Metadata;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace NB.Core.Web.DownloadClient
{
    public class SPYValuationDownloader : BaseDownloader<SPYValuationDataPointAggregate>
    {

        public SPYValuationDownloader(SPYValuationSetting setting)
            : base(setting)
        {

        }

        protected override SPYValuationDataPointAggregate ConvertResult(string content, string ticker = "")
        {
            List<SPYValuationDataPoint> history = new List<SPYValuationDataPoint>(100);

            var resultNode = MyHelper.GetResultTable(content, "(<table id=\"datatable\">.*?</table>)", "//table");
            ParseTable(history, resultNode, ticker, "");
            return new SPYValuationDataPointAggregate((Setting as SPYValuationSetting).Valuationtype, history.ToArray(), ticker);
        }

        private static void ParseTable(List<SPYValuationDataPoint> metricsHistory, XParseElement sourceNode, string symbol, string xPath)
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
                            var data = new SPYValuationDataPoint();
                            foreach (var property in data.GetType().GetProperties())
                            {
                                xpath = property.GetCustomAttributes(typeof(XPathAttribute), false).FirstOrDefault() as XPathAttribute;
                                if (xpath == null)
                                    continue;
                                targetNode = XPath.GetElement(xpath.Path, node);
                                var val = targetNode.Value;

                                if (val.Contains("estimate"))
                                {
                                    val = MyHelper.ExtractPattern(val,@"(\d*\.\d*).*");
                                }
                                value = Convert.ChangeType(val, property.PropertyType);
                                property.SetValue(data, value);
                            }

                            metricsHistory.Add(data);
                        }
                    }
                }
            }
        }
    }
}
