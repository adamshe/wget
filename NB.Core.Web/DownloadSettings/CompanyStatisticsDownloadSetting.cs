using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadSettings
{
    public class CompanyStatisticsDownloadSetting : BaseSetting
    {
        public CompanyStatisticsDownloadSetting()
        {
            Ticker = string.Empty;
        }

        public CompanyStatisticsDownloadSetting(string ticker)
        {
            Ticker = ticker;
        }

        protected sealed override string UrlStr
        {
            get { return "http://finance.yahoo.com/q/ks?s={0}"; }
        }

        public override string GetUrl()
        {
            if (this.Ticker == string.Empty) { throw new ArgumentException("Ticker is empty.", "Ticker"); }
            return GetUrl ( this.Ticker);
        }

        public override string GetUrl(string ticker)
        {
            return string.Format(UrlStr, ticker);
        }

        public override string GetTickerFromUrl(string url)
        {
            var ticker = MyHelper.ExtractPattern(url, @".*/ks\?s=(?<ticker>\w*)");
            return ticker;
        }
       
        public override string GetFileName(string ticker)
        {
            return string.Format("{0}-{1}-companyStatistics.txt", ticker, DateTime.Now.ToShortDateString());
        }

        public override string GetFileName()
        {
            return GetFileName(this.Ticker);
        }

       
    }
}
