﻿using NB.Core.Web.Utility;
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
	public class FinvizEarningCalendarSetting: BaseSetting
	{
		public const string UrlStr = "https://query.yahooapis.com/v1/public/yql?q=select * from html where url=\'http://www.finviz.com/quote.ashx?t={0}\' and xpath=\'/html/body//table[@class=\"snapshot-table2\"]//tr[11]/td[6]\'";

		public FinvizEarningCalendarSetting(string ticker="SPY")
		{
			Ticker = ticker;
		}

		public IEnumerable<string> GetUrls(string symbols)
		{
			var tickers = MyHelper.GetStringToken(symbols, new string[] { ";", "," });
			foreach (var ticker in tickers)
			{
				yield return string.Format(UrlStr, ticker);
			}
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
			//quote.ashx?t={0}
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
