using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.Core.Web.DownloadClient;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using NB.Core.Web.DownloadSettings;
using System.IO;

namespace NB.Core.Web.DownloadClient
{
    public class FinvizEarningCalendarDownloader : BaseDownloader<TickerEarningDate>
    {
        public FinvizEarningCalendarDownloader(BaseSetting setting)
            : base(setting)
        {

        }

        protected override TickerEarningDate ConvertResult(StreamReader sr, string ticker = "")
        {
            throw new NotImplementedException("stream is only for csv files.");
        }

        protected override TickerEarningDate ConvertResult(string contentStr, string ticker="")
        {
            XParseDocument doc = MyHelper.ParseXmlDocument(contentStr);
#if DEBUG
            Console.WriteLine(contentStr);
#endif
            
            XParseElement resultNode = XPath.GetElement("//results/td/strong", doc);
            if (resultNode == null)
                return new TickerEarningDate { Ticker=ticker };

            var dateStr = resultNode.Value;
            var date = MyHelper.ConvertEarningDate(dateStr);

            return new TickerEarningDate { Ticker=ticker, EarningDate = date };
        }
    }
}
