using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadSettings
{
    public class YahooValuationSetting : BaseSetting
    {
        public override string GetUrl()
        {
            return UrlStr;
        }

        public override string GetUrl(string ticker)
        {
            return string.Format("http://biz.yahoo.com/rr/?s={0}&d=p%2Frr", ticker);
        }

        protected override string UrlStr
        {
            get { return "http://biz.yahoo.com/p/s_conameu.html"; }
        }

        public override string GetTickerFromUrl(string url)
        {
            var ticker = MyHelper.ExtractPattern(url, @".*\?s=(?<ticker>\w*)&d=.*Frr$");
            return ticker;
        }
    }
}
