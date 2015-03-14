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
    public class MorningStartValuationDownloader : BaseDownloader<MorningStarValuation>
    {

        public MorningStartValuationDownloader(BaseSetting setting)
            : base(setting)
        {

        }

        protected override MorningStarValuation ConvertResult(string content, string ticker = "")
        {
            var valuation = new MorningStarValuation(ticker);

            XParseElement resultNode;
            var mySetting = Setting as MorningStarValuationSetting;
            if (mySetting.IsForwardValuation)
            {
                resultNode = MyHelper.GetResultTable(content, "(<table id=\"forwardValuationTable\".*>.*?</table>)", "//table/tbody");
                ParseTable(valuation, resultNode, "Forward", "");
            }
            else
            {
                resultNode = MyHelper.GetResultTable(content, "(<table id=\"currentValuationTable\".*>.*?</table>)", "//table/tbody");
                ParseTable(valuation, resultNode, "Current", "");
            }
           
            return valuation;
        }

        private static void ParseTable(MorningStarValuation currentValuation, XParseElement sourceNode, string mark, string xPath)
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
                var data = currentValuation;
                foreach (var property in data.GetType().GetProperties())
                {
                    xpath = property.GetCustomAttributes(typeof(XPathAttribute), false).FirstOrDefault() as XPathAttribute;
                    if (xpath == null || xpath.Name != mark)
                        continue;

                    targetNode = XPath.GetElement(xpath.Path, resultNode);
                    var val = targetNode.Value;

                    if (val == "—") continue;

                    value = Convert.ChangeType(val, property.PropertyType);
                    property.SetValue(data, value);
                }
                /*
                var elements = resultNode.Elements();
                foreach (XParseElement node in elements)
                {
                    if (node.Name.LocalName == "tr")
                    {
                        cnt++;
                        if (cnt > 1) // skip row header
                        {
                            var data = currentValuation;
                            foreach (var property in data.GetType().GetProperties())
                            {                                
                                xpath = property.GetCustomAttributes(typeof(XPathAttribute), false).FirstOrDefault() as XPathAttribute;
                                if (xpath == null || xpath.Name != mark)
                                    continue;
                                
                                targetNode = XPath.GetElement(xpath.Path, node);
                                var val = targetNode.Value;
                               
                                value = Convert.ChangeType(val, property.PropertyType);
                                property.SetValue(data, value);
                            }
                           
                        }
                    }
                }
                 */
            }
        }
    }
}
