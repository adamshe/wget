using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadSettings
{
    public class NasdaqHoldingSetting : BaseSetting
    {
        public NasdaqHoldingSetting(string ticker)
        {
            Ticker = ticker;
        }

        public override string GetUrl()
        {
            return GetUrl(Ticker);
        }

        public override string GetUrl(string ticker)
        {
            return string.Format(UrlStr, ticker);
        }

        protected override string UrlStr
        {
            //http://www.nasdaq.com/symbol/{0}/institutional-holdings
            ///quotes/detail-institutional-holdings.aspx?selected=SGEN&page=1&type=
            get { return "http://www.nasdaq.com/quotes/detail-institutional-holdings.aspx?selected={0}&page=1&type="; }
        }

        public override string GetTickerFromUrl(string url)
        {
            var ticker = MyHelper.ExtractPattern(url, @".*/symbol/(?<ticker>\w*)/institutional-holdings$");
            return ticker;
        }
    }
}
