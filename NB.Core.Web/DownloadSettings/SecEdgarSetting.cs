using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadSettings
{
    public class SecEdgarSetting : BaseSetting
    {
        public override string GetUrl()
        {
            return UrlStr;
        }

        public override string GetUrl(string ticker)
        {
            return UrlStr;
        }

        protected override string UrlStr
        {
            get { return "http://www.sec.gov/Archives/edgar/data/1263508/000114420415015461/0001144204-15-015461-index.htm"; }
        }

        public override string GetTickerFromUrl(string url)
        {
            return "SEC";
        }
    }
}
