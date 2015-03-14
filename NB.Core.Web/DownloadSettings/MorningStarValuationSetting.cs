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
	public class MorningStarValuationSetting: BaseSetting
	{
		public MorningStarValuationSetting(string ticker = "SPY")
		{
			Ticker = ticker;
			IsForwardValuation = false;
		}

		protected sealed override string UrlStr
		{
			get 
			{
				if (IsForwardValuation)
					return "http://financials.morningstar.com/valuation/forward-valuation-list.action?&t={0}&region=usa&culture=en-US";

				return "http://financials.morningstar.com/valuation/current-valuation-list.action?&t={0}&region=usa&culture=en-US"; 
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
			var ticker = MyHelper.ExtractPattern(url, @".*current-valuation-list\.action\?&t=XNAS:(?<ticker>\w*)&.*");
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

		public bool IsForwardValuation { get; set; }
	}
}
