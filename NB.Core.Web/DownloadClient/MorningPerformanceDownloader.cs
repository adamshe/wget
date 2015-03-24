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
    public class MorningStartPerformanceDownloader : BaseDownloader<PerformanceDataAggregate>
    {

        public MorningStartPerformanceDownloader(BaseSetting setting)
            : base(setting)
        {

        }

        protected override PerformanceDataAggregate ConvertResult(string content, string ticker = "")
        {
            var aggregate = new PerformanceDataAggregate(ticker);

            XParseDocument resultNode;
            var mySetting = Setting as MorningStarValuationSetting;
           
            resultNode = MyHelper.GetResultTable(content, 1, "<table class=\"r_table3  width955px print97\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"width:955px\">");
            ParseTable ( aggregate.StockPerformance, resultNode, "Stock" );
            ParseTable ( aggregate.Industryformance, resultNode, "Industry");
            ParseTable ( aggregate.SP500formance, resultNode, "SP500");


            return aggregate;
        }

        private static void ParseTable(PerformanceData perfData, XParseDocument resultNode, string name)
        {           
            XParseElement targetNode;
            XPathAttribute xpath;
            object value;
            if (resultNode != null)
            {
                var data = perfData;
                foreach (var property in data.GetType().GetProperties())
                {
                    xpath = property.GetCustomAttributes(typeof(XPathAttribute), false).Where(attr => ((XPathAttribute)attr).Name == name).FirstOrDefault() as XPathAttribute;

                    targetNode = XPath.GetElement(xpath.Path, resultNode);
                    var val = targetNode.Value;

                    value = Convert.ChangeType(val, property.PropertyType);
                    property.SetValue(data, value);
                }               
            }
        }
    }
}
