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
	public class MorningStarPerformanceSetting: BaseSetting
	{
		public MorningStarPerformanceSetting(string ticker = "SPY")
		{
			Ticker = ticker;
		}

		protected sealed override string UrlStr
		{
			//http://performance.morningstar.com/stock/performance-return.action?t={0}&region=usa&culture=en-US
			//"http://performance.morningstar.com/funds/etf/total-returns.action?t={0}"
			get
			{
				return "http://performance.morningstar.com/Performance/stock/trailing-total-returns.action?&t={0}&region=usa&culture=en-US&cur=&ops=clear&s=0P00001MK8&ndec=2&ep=true&align=d&annlz=true";
					// this is for years y=?
					//"http://performance.morningstar.com/Performance/stock/performance-history-1.action?&t={0}&region=usa&culture=en-US&cur=&ops=clear&s=0P00001MK8&ndec=2&ep=true&align=m&y=10&type=growth"; 
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
			var ticker = MyHelper.ExtractPattern(url, @".*\?&t=(?<ticker>\w*)");
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
