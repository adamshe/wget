using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Models.Metadata;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using NB.Core.Web.Extensions;
using NB.Core.Web.Models.Enums;
using System.Reflection;

namespace NB.Core.Web.DownloadClient
{
    public class YahooValuationDownloader : BaseDownloader<StockDataPointAggregate>
    {
        private const string _baseUrl = "http://biz.yahoo.com/p/";
        public YahooValuationDownloader(BaseSetting setting): base(setting)
        {

        }

        protected override StockDataPointAggregate ConvertResult(string contentStr, string ticker = "")
        {
            var aggregate = new StockDataPointAggregate(ticker);

            if (!string.IsNullOrEmpty(contentStr))
            {
                var normalizeContent = contentStr;//.Replace("\r\n", "").Replace("\t", "");
                /*due to attribute without quotes, can't be parsed it as xml*/
                string startPattern = "<table";
                string endPattern = "</table>";
                int startIndex = normalizeContent.IndexOfOccurence(startPattern, 5);
                int endIndex = normalizeContent.IndexOf(endPattern, startIndex);
                normalizeContent = normalizeContent.Substring(startIndex, endIndex - startIndex + endPattern.Length);
                var tempnormalizeContent = normalizeContent.Replace("\r\n", " ").Replace("\t", " ").Replace("\n"," ");
                normalizeContent = MyHelper.FixAttributes(tempnormalizeContent);
                XParseDocument doc = MyHelper.ParseXmlDocument(normalizeContent);
                var resultNode = XPath.GetElement("/table", doc);
                ParseTable(aggregate, resultNode, ticker, "");
                return aggregate;
            }
            return null;
        }

        private void ParseTable(StockDataPointAggregate aggregate, XParseElement sourceNode, string ticker, string xPath)
        {
            var resultNode = sourceNode;
            if (!(string.IsNullOrWhiteSpace(xPath) || string.IsNullOrEmpty(xPath)))
                resultNode = XPath.GetElement(xPath, sourceNode);
            int rowNum = 0;
            float tempVal;
            XPathAttribute xpath;
            object value;
            XParseElement targetNode, description;
            ValuationDataPoint data;
            string extractVal;
            if (resultNode != null)
            {
                foreach (XParseElement row in resultNode.Elements())
                {
                    if (row.Name.LocalName == "tr")
                    {
                        rowNum++;
                        if (rowNum > 1 && rowNum <= 3) // skip row header
                        {
                            description = XPath.GetElement("/td[1]/font[1]/b", row);
                            if (description != null)
                            {
                                if (description.Value.Contains("Sector"))
                                {
                                    data = new ValuationDataPoint();
                                    data.Context = ContextType.Sector;
                                    FillDataPoint(data, row);
                                    aggregate.Sector = data;
                                }
                                else if (description.Value.Contains("Industry"))
                                {
                                    data = new ValuationDataPoint();
                                    data.Context = ContextType.Industry;
                                    FillDataPoint(data, row);
                                    aggregate.Industry = data;
                                }
                            }                            
                        }
                        else if (rowNum > 3)
                        {                            
                            description = XPath.GetElement("/td[1]/font/a[2]", row);
                            if (description != null && description.Value == ticker)
                            {
                                data = new ValuationDataPoint();
                                data.Context = ContextType.Equity;
                                FillDataPoint(data, row);
                                aggregate.Self = data;
                                break;
                            }                            
                        }
                    }
                }
            }
        }

        private static XPathAttribute GetXPathFromAttribute(PropertyInfo property, string attributeName)
        {
            var xpath = property.GetCustomAttributes<XPathAttribute>(false);//.Where(attr => attr.Name==attributeName).FirstOrDefault();
            var count = xpath.Count();
            if (count==1)
                return xpath.FirstOrDefault();
            else if (count > 1)
                return xpath.Where(attr => attr.Name==attributeName).FirstOrDefault();
            else
                return null;
        }

        private static void FillDataPoint(ValuationDataPoint data, XParseElement node)
        {
            XPathAttribute xpath;
            object value;
            XParseElement targetNode;
            string extractVal;
            string temp;
            foreach (var property in data.GetType().GetProperties())
            {
                xpath = GetXPathFromAttribute(property, data.Context.ToString());
                if (xpath == null)
                    continue;

                targetNode = XPath.GetElement(xpath.Path, node);

                if(xpath.Source != string.Empty)
                    temp = targetNode.Attribute(new XParseName(xpath.Source)).Value;
                else
                    temp = targetNode.Value;

                if (temp == "NA")
                    continue;

                extractVal = (xpath.RegexExpression != string.Empty) ?
                    MyHelper.ExtractPattern(temp, xpath.RegexExpression).Replace("\r\n", " ") :
                    temp;


                value = Convert.ChangeType(extractVal, property.PropertyType);
                property.SetValue(data, value);
            }
        }
    }
}
