using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class YahooQuotesResult : QuotesBaseResult
    {
        private YahooQuotesDownloadSettings mSettings = null;
        public YahooQuotesDownloadSettings Settings { get { return mSettings; } }

        public new YahooQuotesData[] Items { 
        get { 
                return base.Items.Cast<YahooQuotesData>().ToArray();
            } 
        }

        public void SortBy (QuoteProperty property)
        {
            try
            {
                base.Items = base.Items.OrderBy(item => ((YahooQuotesData)item)[property] ?? double.MinValue ).ToArray();
            }
            catch (Exception ex)
            {

            }
        }

        internal YahooQuotesResult(YahooQuotesData[] items, YahooQuotesDownloadSettings settings)
            : base(items)
        {
            mSettings = settings;
        }
    }
}
