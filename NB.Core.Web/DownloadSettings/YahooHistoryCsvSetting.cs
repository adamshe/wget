
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NB.Core.Web.Constants;
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
         * 
         * API https://code.google.com/p/yahoo-finance-managed/wiki/csvHistQuotesDownload
         * */

        private string _intervel = Yahoo.Interval.Day;
		private DateTime _start;
		private DateTime _end;

		public YahooHistoryCsvSetting (string ticker, int days=-1000)
		{
			Ticker = ticker;
			_start = DateTime.Now.AddDays(days);
			_end = DateTime.Now;
		}

		protected sealed override string UrlStr
		{
			get { return "http://ichart.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g={7}&ignore=.csv"; ; }
		}

		public sealed override IEnumerable<string> GetUrls(string symbols)
		{
			var tickers = MyHelper.GetStringToken(symbols, new string[] { ";", "," });
			foreach (var ticker in tickers)
			{
				yield return GetUrl(ticker);
			}            
		}

		[DataType(DataType.Date)]
		public DateTime Start { get { return _start; } set { _start = value; } }

		[DataType(DataType.Date)]
		public DateTime End { get { return _end; } set{ _end = value;} }

		public string Interval
		{
			get { return _intervel; }
			set { _intervel = value; }
		}

		public override string GetUrl()
		{
			return GetUrl(Ticker);
		}

		public override string GetUrl(string ticker)
		{
			DateTime yahooStartDate = Start.AddMonths(-1);
			DateTime yahooEndDate = End.AddMonths(-1);
			return string.Format(UrlStr, ticker,
					yahooStartDate.Month, yahooStartDate.Day, yahooStartDate.Year,
					yahooEndDate.Month, yahooEndDate.Day, yahooEndDate.Year,
					_intervel);
		}

		public override string GetTickerFromUrl(string url)
		{
			var ticker = MyHelper.ExtractPattern(url, @".*\?s=(?<ticker>\^?\w*)&.*");
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
