using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadSettings
{
    public class TrefisSetting : BaseSetting
    {
        public override string GetUrl()
        {
            return "http://www.trefis.com/companies";
        }

        public override string GetUrl(string ticker)
        {
            return GetUrl();
        }

        public override string GetTickerFromUrl(string url)
        {
            var ticker = MyHelper.ExtractPattern(url, @".*\?hm=(?<ticker>\^?\w*).trefis$");
            return ticker;
        }
       
        public override string GetFileName(string ticker)
        {
            return base.GetFileName(ticker);
        }

        public override string GetFileName()
        {
            return base.GetFileName();
        }
    }
}
