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
	public class SPYValuationDownloadSetting: BaseSetting
	{
		public SPYValuationDownloadSetting(string ticker = "SPY")
		{
			Ticker = ticker;
			Valuationtype = ValuationType.PE;
		}

		public ValuationType Valuationtype { get; set; }

		protected sealed override string UrlStr
		{
			get 
			{
				switch(Valuationtype)
				{
					case  ValuationType.PS:
						return "http://www.multpl.com/s-p-500-price-to-sales/table/by-quarter";

					case ValuationType.PB:
						return "http://www.multpl.com/s-p-500-price-to-book/table/by-quarter";

					default:
						return "http://www.multpl.com/table?f=m";
				}               
			}
		}

		public override string GetFileName(string ticker)
		{
			return string.Format(@"{0}\{1}-{2}-{3}.txt", Directory.GetCurrentDirectory(), ticker, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture), Valuationtype.ToString());
		}

		public override string GetFileName()
		{
			return GetFileName(Ticker);
		}

		public override string GetTickerFromUrl(string url)
		{
			return Ticker;
		}

		public override string GetUrl()
		{
			return UrlStr;
		}

		public override string GetUrl(string ticker)
		{
			return GetUrl();
		}
	}
}
