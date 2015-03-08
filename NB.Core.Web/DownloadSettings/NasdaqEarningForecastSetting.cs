using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadSettings
{
    public class NasdaqEarningForecastSetting : BaseSetting
    {
        public NasdaqEarningForecastSetting()
        {
            this.Ticker = string.Empty;
        }

        public NasdaqEarningForecastSetting(string ticker)
        {
            this.Ticker = ticker;
        }

        public sealed override string GetUrl()
        {
            return GetUrl(this.Ticker);
        }

        public sealed override string GetUrl(string ticker)
        {
            if (string.IsNullOrEmpty(ticker)) { throw new ArgumentException("Ticker is empty.", "Ticker"); }
            return string.Format(UrlStr, ticker);
        }

        protected sealed override string UrlStr
        {
            get { return "http://www.nasdaq.com/symbol/{0}/earnings-forecast"; }
        }

        public override string GetTickerFromUrl(string url)
        {
            var ticker = MyHelper.ExtractPattern(url, @".*symbol/(?<ticker>\w*)/earnings-forecast$");
            return ticker;
        }
    }
}
