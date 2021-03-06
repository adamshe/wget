﻿using NB.Core.Web.Utility;

namespace NB.Core.Web.DownloadSettings
{
    public class TrefisSetting : BaseSetting
    {
        public override string GetUrl()
        {
            return UrlStr;
        }

        public override string GetUrl(string ticker)
        {
            return GetUrl();
        }

        protected sealed override string UrlStr
        {
            get { return "http://www.trefis.com/companies"; }
        }


        public override string GetTickerFromUrl(string url)
        {
            var ticker = MyHelper.ExtractPattern(url, @".*\?hm=(?<ticker>\^?\w*).trefis$");
            return ticker;
        }
       
    }
}
