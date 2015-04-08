using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadSettings
{
	public class AnalystRatingsSetting: BaseSetting
	{
			/*"https:/table/query.yahooapis.com/v1/public/yql?q=select * from html where url=\'http:/table/www.finviz.com/quote.ashx?t={0}\' and xpath=\'/html/body/table/table[@class=\"snapshot-table2\"]/tr\'";*/
		private string _urlStr;
		public AnalystRatingsSetting(string ticker = "SPY")
		{
			Ticker = ticker;
		}

		protected sealed override string UrlStr
		{            
			get 
			{
                String redirectURL = "http://www.analystratings.net/pages/search.aspx?query={0}";
                //var request = (HttpWebRequest)HttpWebRequest.Create(redirectURL);
                //request.Method = "POST";
                //request.AllowAutoRedirect = true;
                //request.ContentType = "application/x-www-form-urlencoded";
                //var postData = Encoding.ASCII.GetBytes("txtSearch=AAPL");
                //using (var stream = request.GetRequestStream())
                //{
                //    stream.Write(postData, 0, postData.Length);
                //}
				
                //var response = (HttpWebResponse)request.GetResponse();
                //if (response.StatusCode == HttpStatusCode.Redirect)                 
                //      redirectURL = response.Headers["Location"];

				return redirectURL; 
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
			var ticker = MyHelper.ExtractPattern(url, @".*/(?<ticker>\w*)/$");
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
