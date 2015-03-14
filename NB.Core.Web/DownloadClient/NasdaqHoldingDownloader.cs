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

namespace NB.Core.Web.DownloadClient
{
    public class NasdaqHoldingDownloader : BaseDownloader<HoldingDataPointAggregate>
    {
        public NasdaqHoldingDownloader(BaseSetting setting) : base(setting)
        {

        }
        protected override HoldingDataPointAggregate ConvertResult(string contentStr, string ticker = "")
        {
            var aggregate = new HoldingDataPointAggregate(ticker);

            if (!string.IsNullOrEmpty(contentStr))
            {
                var normalizeContent = contentStr;//.Replace("\r\n", "").Replace("\t", "");
                /*due to attribute without quotes, can't be parsed it as xml*/
                string startPattern = "<form";
                string endPattern = "</form>";
                string startTablePattern = "<table>";
                string endTablePattern = "</table>";

                int startIndex = normalizeContent.IndexOfOccurence(startPattern, 4);
                int endIndex = normalizeContent.IndexOf(endPattern, startIndex);
                normalizeContent = normalizeContent.Substring(startIndex, endIndex - startIndex + endPattern.Length);
                var tempnormalizeContent = normalizeContent.Replace("\r\n", " ").Replace("\t", " ").Replace("\n", " ");
                normalizeContent = MyHelper.FixAttributes(tempnormalizeContent);
                XParseDocument doc = MyHelper.ParseXmlDocument(normalizeContent);
                string temp;
                for (int i = 1; i <= 4; i++)
                {
                    startIndex = normalizeContent.IndexOfOccurence(startTablePattern, i);
                    endIndex = normalizeContent.IndexOf(endTablePattern, startIndex);
                    temp = normalizeContent.Substring(startIndex, endIndex - startIndex + endTablePattern.Length);
                    doc = MyHelper.ParseXmlDocument(temp);
                    var ownershipSummaryTable = XPath.GetElement("//table", doc);
                    ParseTable(aggregate, ownershipSummaryTable);
                }
                //var activePositionTable = XPath.GetElement("//table[2]", doc);
                //var newSoldoutTable = XPath.GetElement("//table[3]", doc);
                //var holdingDetail = XPath.GetElement("//table[4]", doc);
             
                return aggregate;
            }
            return null;
        }

        private void ParseTable (HoldingDataPointAggregate holding, XParseElement table)
        {

        }
    }
}
