using System;
using System.Collections.Generic;

namespace wget
{
	class YahooDownloadParam
	{
		public const string MatchStr = @".*\?s=(?<ticker>\w*)&.*";
		public const string YahooCsvStr = "http://ichart.finance.yahoo.com/table.csv?s={0}&d=8&e=12&g=d&a=0&b=29&c={1}&f={2}&ignore=.csv";
		public YahooDownloadParam (string[] args)
		{
			Symbols = args[0];
			StartYear = args[1];
			EndYear = args.Length <= 2 ? DateTime.Now.Year.ToString() : args[2];

		}
		public string Symbols { get; set; }

		public string StartYear { get; set; }

		public string EndYear { get; set; }


		public IEnumerable<string> GetUrls ()
		{
			var tickers = Symbols.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var ticker in tickers)
			{
				yield return string.Format(YahooCsvStr, ticker, StartYear, EndYear);
			}
		}
	}
}
