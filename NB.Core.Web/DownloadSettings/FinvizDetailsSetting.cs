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
	public class FinvizDetailsSetting: BaseSetting
	{
			/*"https:/table/query.yahooapis.com/v1/public/yql?q=select * from html where url=\'http:/table/www.finviz.com/quote.ashx?t={0}\' and xpath=\'/html/body/table/table[@class=\"snapshot-table2\"]/tr\'";*/

		public FinvizDetailsSetting(string ticker = "SPY")
		{
			Ticker = ticker;
		}

		protected sealed override string UrlStr
		{
			get { return "http://www.finviz.com/quote.ashx?t={0}"; }
		}

		public override string GetFileName(string ticker)
		{
			return string.Format(@"{0}\{1}-{2}.txt", Directory.GetCurrentDirectory(), ticker, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
		}

		public override string GetFileName()
		{
			return string.Format(@"{0}\{1}-{2}.txt", Directory.GetCurrentDirectory(), Ticker, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
		}

		public override string GetTickerFromUrl(string url)
		{
			var match = Regex.Match(url, @".*quote.ashx\?t=(?<ticker>\w*)\'\s.*");
			var ticker = match.Groups["ticker"].Value;
			return ticker;
		}

		public override string GetUrl()
		{
			return string.Format(UrlStr, Ticker);
		}

		public override string GetUrl(string ticker)
		{
			return string.Format(UrlStr, ticker);
		}
	}
}
