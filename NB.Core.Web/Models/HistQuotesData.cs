using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
	public class HistQuotesData
	{
		/// <summary>
		/// The startdate of the period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public DateTime TradingDate { get; set; }
		/// <summary>
		/// The first value in trading period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double Open { get; set; }
		/// <summary>
		/// The highest value in trading period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double High { get; set; }
		/// <summary>
		/// The lowest value in trading period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double Low { get; set; }
		/// <summary>
		/// The last value in trading period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double Close { get; set; }
		/// <summary>
		/// The last value in trading period in relation to share splits.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double CloseAdjusted { get; set; }
		/// <summary>
		/// The traded volume.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public long Volume { get; set; }
		/// <summary>
		/// The close value of the previous HistQuoteData in chain.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double PreviousClose { get; set; }
	}
}
