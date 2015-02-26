using NB.Core.Web.Enums;
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadSettings
{
    public class YahooQuotesDownloadSettings : BaseSetting
    {
        public const string UrlStr = "http://download.finance.yahoo.com/d/quotes.csv?s=";
        private QuoteProperty[] _quoteProperties;
        private string[] ids;

        public YahooQuotesDownloadSettings()
        {
            this.IDs = new string[] { };
            this.Properties = new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly };
        }

        public YahooQuotesDownloadSettings(string id)
        {
           // this.TextEncoding = System.Text.Encoding.UTF8;
            this.IDs = new string[] { id };
            this.Properties = new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly };
        }

        public YahooQuotesDownloadSettings(string id, QuoteProperty[] properties)
        {
            this.IDs = new string[] { id };
            this.Properties = properties;
        }

        public string[] IDs
        {
            get { return ids; }
            set
            {
                if (ids != null)
                {
                    var list = new string[ids.Length + value.Length];
                    ids.CopyTo(list, 0);
                    value.CopyTo(list, ids.Length);
                    ids = list;
                }
                else
                    ids = value;
            }
        }

        public QuoteProperty[] Properties
        {
            get { return _quoteProperties; } 
            set 
            {
                if (_quoteProperties != null)
                {
                    var list = new QuoteProperty[_quoteProperties.Length + value.Length];
                    _quoteProperties.CopyTo(list, 0);
                    value.CopyTo(list, _quoteProperties.Length);
                    _quoteProperties = list;
                }
                else
                    _quoteProperties = value;
            }
        }

        public override string GetUrl()
        {
            if (this.IDs == null || this.IDs.Length == 0)
            {
                throw new NotSupportedException("An empty id list will not be supported.");
            }
            else
            {
                System.Text.StringBuilder ids = new System.Text.StringBuilder();
                foreach (string s in this.IDs)
                {
                    ids.Append(MyHelper.CleanYqlParam(s));
                    ids.Append('+');
                }
                String url = UrlStr + Uri.EscapeDataString(ids.ToString()) + "&f=" + FinanceHelper.CsvQuotePropertyTags(this.Properties) + "&e=.csv";
                return url;
            }
        }

        public override string GetUrl(string ticker)
        {
            String url = UrlStr + Uri.EscapeDataString(ticker) + "&f=" + FinanceHelper.CsvQuotePropertyTags(this.Properties) + "&e=.csv";
            return url;
        }

        public override string GetTickerFromUrl(string url)
        {
            var match = Regex.Match(url, @".*quotes.csv\?s=(?<ticker>\w*)&f=.*");
            var ticker = match.Groups["ticker"].Value;
            return ticker;
        }

        public override string GetFileName(string ticker)
        {
            return string.Format(@"{0}\{1}-{2}-Quote.txt", Directory.GetCurrentDirectory(), ticker, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
        }

        public override string GetFileName()
        {
            return string.Format(@"{0}\{1}-{2}-Quote.txt", Directory.GetCurrentDirectory(), Ticker, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
        }
    }
}
