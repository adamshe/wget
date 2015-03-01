
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
namespace NB.Core.Web.DownloadSettings
{
	public class YahooHistoryCsvSetting : BaseSetting
	{
		/*
		//http://table.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g=d&ignore=.csv
		 * 
		 * You can ask the financial api, in some way like that:

http://finance.yahoo.com/d/quotes.csv?s=
And the historical data you can get by:

http://finance.yahoo.com/q/hp?s=
		 * */
		public const string YahooCsvStr = "http://ichart.finance.yahoo.com/table.csv?s={0}&d=8&e=12&g=d&a=0&b=29&c={1}&f={2}&ignore=.csv";
		public YahooHistoryCsvSetting (string ticker)
		{
			Ticker = ticker;
		}

		public IEnumerable<string> GetUrls(string symbols)
		{
			var fixSymbols = MyHelper.FixString(symbols);
			var tickers = fixSymbols.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var ticker in tickers)
			{
				yield return string.Format(YahooCsvStr, ticker, StartYear ?? (DateTime.Now.Year - 4).ToString(), EndYear ?? DateTime.Now.Year.ToString());
			}
		}
		public string StartYear { get; set; }

		public string EndYear { get; set; }

		public override string GetUrl()
		{
			return string.Format(YahooCsvStr, Ticker, StartYear, EndYear);
		}

		public override string GetUrl(string ticker)
		{
			return string.Format(YahooCsvStr, ticker, DateTime.Now.Year - 4, DateTime.Now.Year);
		}

		public override string GetTickerFromUrl(string url)
		{
			var match = Regex.Match(url, @".*\?s=(?<ticker>\^?\w*)&.*");
			var ticker = match.Groups["ticker"].Value;
			return ticker;
		}

		public override string GetFileName(string ticker)
		{
			return string.Format(@"{0}\{1}-{2}.csv", Directory.GetCurrentDirectory(), ticker, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
		}

		public override string GetFileName()
		{
			return string.Format(@"{0}\{1}-{2}.csv", Directory.GetCurrentDirectory(), Ticker, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
		}
	}
}
