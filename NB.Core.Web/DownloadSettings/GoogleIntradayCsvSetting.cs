using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
/*
 * Copy the link below and paste into the browser
http://www.google.com/finance/getprices?q=SUZLON&x=NSE&i=120&p=1000d&f=d,c,o,h,l&df=cpct&auto=1&ts=1266701290218
q= stock symbol on Google finance
x= exchange symbol
i= interval (here 1=120 means 120 sec (2 minute interval))
p= no of period
f= parameters (day, close, open, high and low)
df= difference (cpct is may be in % change , not sure)
auto =1, and ts value I don’t know.
Here I have chosen period p=1000d to get the maximum data possible. you can choose your own set of value depending upon your need.
After getting the data copy and paste the data into a spreadsheet and format little bit to make it usable.
Though I have found it works pretty well for me. IF it has any error then any comment and suggestion is invited to improve it.  Any way it is free so giving a try is worthwhile since you have nothing to lose. I hope you will like it. Enjoy the free data mining
 * 
 * http://www.quantshare.com/sa-426-6-ways-to-download-free-intraday-and-tick-data-for-the-us-stock-market
 */
/*ten days intraday*/
namespace NB.Core.Web.DownloadSettings
{
	public class GoogleIntradayCsvSetting: BaseSetting
	{
		public const string UrlStr = @"http://www.google.com/finance/getprices?q={0}&i={1}&p=1000d&f=d,c,o,h,l";

		public GoogleIntradayCsvSetting(string ticker = "SPY")
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
			return string.Format(@"{0}\{1}-{2}-intraday.txt", Directory.GetCurrentDirectory(), ticker, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
		}

		public override string GetFileName()
		{
			return string.Format(@"{0}\{1}-{2}-intraday.txt", Directory.GetCurrentDirectory(), Ticker, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
		}

		public override string GetTickerFromUrl(string url)
		{
			var ticker = MyHelper.ExtractPattern(url, @".*getprices\?q=(?<ticker>\w*)&.*");
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
