using NB.Core.Web.Utility;

namespace NB.Core.Web.DownloadSettings
{
    public class CpiDataSetting : BaseSetting
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
            get { return "http://inflationdata.com/Inflation/Consumer_Price_Index/HistoricalCPI.aspx?reloaded=true"; }
        }


        public override string GetTickerFromUrl(string url)
        {            
            return "US CPI Data";
        }
       
    }
}
