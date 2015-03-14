using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NB.Core.Web.DownloadClient
{
    public class NasdaqEarningForecastDownloader : BaseDownloader<NasdaqEarningForecastAggregate>
    {
        public NasdaqEarningForecastDownloader(BaseSetting setting) : base (setting)
        {

        }

        public NasdaqEarningForecastDownloader()
            : this(new NasdaqEarningForecastSetting())
        {
        }

        protected sealed override NasdaqEarningForecastAggregate ConvertResult(string contentStr, string ticker = "")
        {
            List<NasdaqEarningForecastData> yearly = new List<NasdaqEarningForecastData>(10);
            List<NasdaqEarningForecastData> quarterly = new List<NasdaqEarningForecastData>(10);
            string pattern = @"<title>.*\((\w*)\).*</title>";

            if (!string.IsNullOrEmpty(contentStr))
            {
                var content = contentStr;

                var matchPattern = "(<div class=\"genTable\">.*?</div>)";
                var match = Regex.Matches(content, matchPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                XParseDocument year = MyHelper.ParseXmlDocument(match[0].Groups[0].Value);
                XParseDocument quarter = MyHelper.ParseXmlDocument(match[1].Groups[0].Value);


                var symbol = Regex.Match(content, pattern).Groups[1].Value;
                var resultNode = XPath.GetElement("//table", year);
                ParseTable(yearly, resultNode, symbol, "");

                resultNode = XPath.GetElement("//table", quarter);
                ParseTable(quarterly, resultNode, symbol, "");

                return new NasdaqEarningForecastAggregate(yearly.ToArray(), quarterly.ToArray(), ticker);
            }
            return null;
        }

        private static void ParseTable(List<NasdaqEarningForecastData> yearly, XParseElement sourceNode, string symbol, string xPath)
        {
            var resultNode = sourceNode;
            if (!(string.IsNullOrWhiteSpace(xPath) || string.IsNullOrEmpty(xPath)))
                resultNode = XPath.GetElement(xPath, sourceNode);
            int cnt = 0;
            float tempVal;
            if (resultNode != null)
            {
                foreach (XParseElement node in resultNode.Elements())
                {
                    if (node.Name.LocalName == "tr")
                    {
                        cnt++;
                        if (cnt > 1) // skip row header
                        {
                            XParseElement tempNode = null;

                            var data = new NasdaqEarningForecastData();
                            data.Ticker = symbol;
                            tempNode = XPath.GetElement("/td[1]", node);
                            if (tempNode != null)
                                data.FiscalEnd = HttpUtility.HtmlDecode(tempNode.Value);

                            tempNode = XPath.GetElement("/td[2]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.ConsensusEpsForecast = tempVal;

                            tempNode = XPath.GetElement("/td[3]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.HighEpsForecast = tempVal;

                            tempNode = XPath.GetElement("/td[4]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.LowEpsForecast = tempVal;

                            tempNode = XPath.GetElement("/td[5]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.NumberOfEstimate = (int)tempVal;

                            tempNode = XPath.GetElement("/td[6]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.NumOfRevisionUp = (int)tempVal;

                            tempNode = XPath.GetElement("/td[7]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.NumOfrevisionDown = (int)tempVal;

                            yearly.Add(data);
                        }
                    }
                }
            }
        }
    }
}
