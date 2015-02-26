// ******************************************************************************
// ** 
// **  Yahoo! Managed
// **  Written by Marius Häusler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius Häusler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NB.Core.Web.Enums;
using NB.Core.Web.Models;
using NB.Core.Web.Interfaces;
using NB.Core.Web.Xml;


namespace NB.Core.Web.Utility
{
	internal abstract class FinanceHelper
	{
		public const string NameOptionSymbol = "symbol";
		public const string NameOptionType = "type";
		public const string NameOptionLastPrice = "lastPrice";
		public const string NameOptionStrikePrice = "strikePrice";
		public const string NameOptionChange = "change";
		public const string NameOptionBid = "bid";
		public const string NameOptionAsk = "ask";
		public const string NameOptionVolume = "vol";
		public const string NameOptionOpenInterest = "openInt";

		public const string NameOptionChangeDir = "changeDir";
		public const string NameQuoteBaseID = "Symbol";
		public const string NameQuoteBaseLastTradePriceOnly = "LastTradePriceOnly";
		public const string NameQuoteBaseChange = "Change";
		public const string NameQuoteBaseOpen = "Open";
		public const string NameQuoteBaseDaysHigh = "DaysHigh";
		public const string NameQuoteBaseDaysLow = "DaysLow";
		public const string NameQuoteBaseVolume = "Volume";
		public const string NameQuoteBaseLastTradeDate = "LastTradeDate";

		public const string NameQuoteBaseLastTradeTime = "LastTradeTime";
		public const string NameHistQuoteDate = "Date";
		public const string NameHistQuoteOpen = "Open";
		public const string NameHistQuoteHigh = "High";
		public const string NameHistQuoteLow = "Low";
		public const string NameHistQuoteClose = "Close";
		public const string NameHistQuoteVolume = "Volume";

		public const string NameHistQuoteAdjClose = "AdjClose";
		public const string NameMarketName = "name";
		public const string NameIndustryID = "id";
		public const string NameCompanySymbol = "symbol";
		public const string NameCompanyCompanyName = "CompanyName";
		public const string NameCompanyStart = "start";
		public const string NameCompanyEnd = "end";
		public const string NameCompanySector = "Sector";
		public const string NameCompanyIndustry = "Industry";
		public const string NameCompanyFullTimeEmployees = "FullTimeEmployees";

		public const string NameCompanyNotAvailable = "NaN";

		private static System.Globalization.CultureInfo mDefaultCulture = new System.Globalization.CultureInfo("en-US");
		public static System.Globalization.CultureInfo DefaultYqlCulture
		{
			get { return mDefaultCulture; }
		}

		public static IEnumerable<string> IIDsToStrings(IEnumerable<IID> idList)
		{
			List<string> lst = new List<string>();
			if (idList != null)
			{
				foreach (IID id in idList)
				{
					if (id != null && id.ID != string.Empty)
						lst.Add(id.ID);
				}
			}
			return lst;
		}
		public static Sector[] SectorEnumToArray(IEnumerable<Sector> values)
		{
			List<Sector> lst = new List<Sector>();
			if (values != null)
			{
				lst.AddRange(values);
			}
			return lst.ToArray();
		}
		public static string[] CleanIDfromAT(IEnumerable<string> enm)
		{
			if (enm != null)
			{
				List<string> lst = new List<string>();
				foreach (string id in enm)
				{
					lst.Add(CleanIndexID(id));
				}
				return lst.ToArray();
			}
			else
			{
				return null;
			}
		}
		public static string CleanIndexID(string id)
		{
			return id.Replace("@", "");
		}
		public static QuoteProperty[] CheckPropertiesOfQuotesData(IEnumerable<YahooQuotesData> quotes, IEnumerable<QuoteProperty> properties)
		{
			List<QuoteProperty> lstProperties = new List<QuoteProperty>();
			if (properties == null)
			{
				return GetAllActiveProperties(quotes);
			}
			else
			{
				lstProperties.AddRange(properties);
				if (lstProperties.Count == 0)
				{
					return GetAllActiveProperties(quotes);
				}
				else
				{
					return lstProperties.ToArray();
				}
			}
		}

		public static QuoteProperty[] GetAllActiveProperties(IEnumerable<YahooQuotesData> quotes)
		{
			List<QuoteProperty> lst = new List<QuoteProperty>();
			if (quotes != null)
			{
				foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
				{
					bool valueIsNotNull = false;
					foreach (YahooQuotesData quote in quotes)
					{
						if (quote[qp] != null)
						{
							valueIsNotNull = true;
							break; // TODO: might not be correct. Was : Exit For
						}
					}
					if (valueIsNotNull)
						lst.Add(qp);
				}
			}
			return lst.ToArray();
		}

		public static double GetStringMillionFactor(string s)
		{
			if (s.EndsWith("T") || s.EndsWith("K"))
			{
				return 1.0 / 1000;
			}
			else if (s.EndsWith("M"))
			{
				return 1;
			}
			else if (s.EndsWith("B"))
			{
				return 1000;
			}
			else
			{
				return 0;
			}
		}
		public static double GetMillionValue(string s)
		{
			double v = 0;
			double.TryParse(s.Substring(0, s.Length - 1), System.Globalization.NumberStyles.Any, mDefaultCulture, out v);
			return v * GetStringMillionFactor(s);
		}
		public static string CleanTd(string value)
		{
			List<char> sb = new List<char>();
			if (value.Length > 0)
			{
				bool allowCopy = true;
				for (int i = 0; i <= value.Length - 1; i++)
				{
					if (value[i] == '<')
					{
						allowCopy = false;
					}
					else if (value[i] == '>')
					{
						allowCopy = true;
						continue;
					}
					if (allowCopy)
						sb.Add(value[i]);
				}
			}
			return new string(sb.ToArray()).Replace("&nbsp;", "");
		}
		public static double ParseToDouble(string s)
		{
			double v = 0;
			double.TryParse(s.Replace("%", ""), System.Globalization.NumberStyles.Any, mDefaultCulture, out v);
			return v;
		}

		public static DateTime ParseToDateTime(string s)
		{
			DateTime d = new DateTime();
			System.DateTime.TryParse(s, mDefaultCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out d);
			return d;
		}
		//public static YCurrencyID YCurrencyIDFromString(string id)
		//{
		//    string idStr = id.ToUpper();
		//    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[A-Z][A-Z][A-Z][A-Z][A-Z][A-Z]=X");
		//    if (idStr.Length == 8 && regex.Match(idStr).Success)
		//    {
		//        CurrencyInfo b = null;
		//        CurrencyInfo dep = null;
		//        string baseStr = idStr.Substring(0, 3);
		//        string depStr = idStr.Substring(3, 3);
		//        foreach (Support.CurrencyInfo cur in WorldMarket.DefaultCurrencies)
		//        {
		//            if (baseStr == cur.ID)
		//            {
		//                b = new Support.CurrencyInfo(cur.ID, cur.Description);
		//            }
		//            else if (depStr == cur.ID)
		//            {
		//                dep = new Support.CurrencyInfo(cur.ID, cur.Description);
		//            }
		//            if (b != null & dep != null)
		//            {
		//                return new Support.YCurrencyID(b, dep);
		//            }
		//        }

		//        return null;
		//    }
		//    else
		//    {
		//        return null;
		//    }
		//}

		public static string GetChartImageSize(ChartImageSize value)
		{
			return value.ToString().Substring(0, 1).ToLower();
		}
		public static string GetChartTimeSpan(ChartTimeSpan value)
		{
			if (value == ChartTimeSpan.cMax)
			{
				return "my";
			}
			else
			{
				return value.ToString().Replace("c", "").ToLower();
			}
		}
		public static string GetChartType(ChartType value)
		{
			return value.ToString().Substring(0, 1).ToLower();
		}
		public static string GetChartScale(ChartScale value)
		{
			if (value == ChartScale.Arithmetic)
			{
				return "off";
			}
			else
			{
				return "on";
			}
		}
		public static string GetMovingAverageInterval(MovingAverageInterval value)
		{
			return value.ToString().Replace("m", "");
		}
		public static string GetTechnicalIndicatorsI(TechnicalIndicator value)
		{
			switch (value)
			{
				case TechnicalIndicator.Bollinger_Bands:
					return value.ToString().Substring(0, 1).ToLower() + ',';
				case TechnicalIndicator.Parabolic_SAR:
					return value.ToString().Substring(0, 1).ToLower() + ',';
				case TechnicalIndicator.Splits:
					return value.ToString().Substring(0, 1).ToLower() + ',';
				case TechnicalIndicator.Volume:
					return value.ToString().Substring(0, 1).ToLower() + ',';
				default:
					return string.Empty;
			}
		}
		public static string GetTechnicalIndicatorsII(TechnicalIndicator value)
		{
			switch (value)
			{
				case TechnicalIndicator.MACD:
					return "m26-12-9,";
				case TechnicalIndicator.MFI:
					return "f14,";
				case TechnicalIndicator.ROC:
					return "p12,";
				case TechnicalIndicator.RSI:
					return "r14,";
				case TechnicalIndicator.Slow_Stoch:
					return "ss,";
				case TechnicalIndicator.Fast_Stoch:
					return "fs,";
				case TechnicalIndicator.Vol:
					return "v,";
				case TechnicalIndicator.Vol_MA:
					return "vm,";
				case TechnicalIndicator.W_R:
					return "w14,";
				default:
					return string.Empty;
			}
		}

		public static char GetHistQuotesInterval(HistQuotesInterval item)
		{
			switch (item)
			{
				case HistQuotesInterval.Daily:
					return 'd';
				case HistQuotesInterval.Weekly:
					return 'w';
				default:
					return 'm';
			}
		}

		public static string MarketQuotesRankingTypeString(MarketQuoteProperty rankedBy)
		{
			switch (rankedBy)
			{
				case MarketQuoteProperty.Name:
					return "coname";
				case MarketQuoteProperty.DividendYieldPercent:
					return "yie";
				case MarketQuoteProperty.LongTermDeptToEquity:
					return "qto";
				case MarketQuoteProperty.MarketCapitalizationInMillion:
					return "mkt";
				case MarketQuoteProperty.NetProfitMarginPercent:
					return "qpm";
				case MarketQuoteProperty.OneDayPriceChangePercent:
					return "pr1";
				case MarketQuoteProperty.PriceEarningsRatio:
					return "pee";
				case MarketQuoteProperty.PriceToBookValue:
					return "pri";
				case MarketQuoteProperty.PriceToFreeCashFlow:
					return "prf";
				case MarketQuoteProperty.ReturnOnEquityPercent:
					return "ttm";
				default:
					return string.Empty;
			}
		}
		public static string MarketQuotesRankingDirectionString(System.ComponentModel.ListSortDirection dir)
		{
			if (dir == ListSortDirection.Ascending)
			{
				return "u";
			}
			else
			{
				return "d";
			}
		}

		public static string CsvQuotePropertyTags(QuoteProperty[] properties)
		{
			//https://code.google.com/p/yahoo-finance-managed/wiki/CSVAPI
			System.Text.StringBuilder symbols = new System.Text.StringBuilder();
			if (properties != null && properties.Length > 0)
			{
				foreach (QuoteProperty qp in properties)
				{
					switch (qp)
					{
						case QuoteProperty.Ask:
							symbols.Append("a0");
							break;
						case QuoteProperty.AverageDailyVolume:
							symbols.Append("a2");
							break;
						case QuoteProperty.AskSize:
							symbols.Append("a5");
							break;
						case QuoteProperty.Bid:
							symbols.Append("b0");
							break;
						case QuoteProperty.AskRealtime:
							symbols.Append("b2");
							break;
						case QuoteProperty.BidRealtime:
							symbols.Append("b3");
							break;
						case QuoteProperty.BookValuePerShare:
							symbols.Append("b4");
							break;
						case QuoteProperty.BidSize:
							symbols.Append("b6");
							break;
						case QuoteProperty.Change_ChangeInPercent:
							symbols.Append('c');
							break;
						case QuoteProperty.Change:
							symbols.Append("c1");
							break;
						case QuoteProperty.Commission:
							symbols.Append("c3");
							break;
						case QuoteProperty.Currency:
							symbols.Append("c4");
							break;
						case QuoteProperty.ChangeRealtime:
							symbols.Append("c6");
							break;
						case QuoteProperty.AfterHoursChangeRealtime:
							symbols.Append("c8");
							break;
						case QuoteProperty.TrailingAnnualDividendYield:
							symbols.Append("d0");
							break;
						case QuoteProperty.LastTradeDate:
							symbols.Append("d1");
							break;
						case QuoteProperty.TradeDate:
							symbols.Append("d2");
							break;
						case QuoteProperty.DilutedEPS:
							symbols.Append("e0");
							break;
						case QuoteProperty.EPSEstimateCurrentYear:
							symbols.Append("e7");
							break;
						case QuoteProperty.EPSEstimateNextYear:
							symbols.Append("e8");
							break;
						case QuoteProperty.EPSEstimateNextQuarter:
							symbols.Append("e9");
							break;
						case QuoteProperty.TradeLinksAdditional:
							symbols.Append("f0");
							break;
						case QuoteProperty.SharesFloat:
							symbols.Append("f6");
							break;
						case QuoteProperty.DaysLow:
							symbols.Append("g0");
							break;
						case QuoteProperty.HoldingsGainPercent:
							symbols.Append("g1");
							break;
						case QuoteProperty.AnnualizedGain:
							symbols.Append("g3");
							break;
						case QuoteProperty.HoldingsGain:
							symbols.Append("g4");
							break;
						case QuoteProperty.HoldingsGainPercentRealtime:
							symbols.Append("g5");
							break;
						case QuoteProperty.HoldingsGainRealtime:
							symbols.Append("g6");
							break;
						case QuoteProperty.DaysHigh:
							symbols.Append("h0");
							break;
						case QuoteProperty.MoreInfo:
							symbols.Append('i');
							break;
						case QuoteProperty.OrderBookRealtime:
							symbols.Append("i5");
							break;
						case QuoteProperty.YearLow:
							symbols.Append("j0");
							break;
						case QuoteProperty.MarketCapitalization:
							symbols.Append("j1");
							break;
						case QuoteProperty.SharesOutstanding:
							symbols.Append("j2");
							break;
						case QuoteProperty.MarketCapRealtime:
							symbols.Append("j3");
							break;
						case QuoteProperty.EBITDA:
							symbols.Append("j4");
							break;
						case QuoteProperty.ChangeFromYearLow:
							symbols.Append("j5");
							break;
						case QuoteProperty.PercentChangeFromYearLow:
							symbols.Append("j6");
							break;
						case QuoteProperty.YearHigh:
							symbols.Append("k0");
							break;
						case QuoteProperty.LastTradeRealtimeWithTime:
							symbols.Append("k1");
							break;
						case QuoteProperty.ChangeInPercentRealtime:
							symbols.Append("k2");
							break;
						case QuoteProperty.LastTradeSize:
							symbols.Append("k3");
							break;
						case QuoteProperty.ChangeFromYearHigh:
							symbols.Append("k4");
							break;
						case QuoteProperty.ChangeInPercentFromYearHigh:
							symbols.Append("k5");
							break;
						case QuoteProperty.LastTradeWithTime:
							symbols.Append("l0");
							break;
						case QuoteProperty.LastTradePriceOnly:
							symbols.Append("l1");
							break;
						case QuoteProperty.HighLimit:
							symbols.Append("l2");
							break;
						case QuoteProperty.LowLimit:
							symbols.Append("l3");
							break;
						case QuoteProperty.DaysRange:
							symbols.Append('m');
							break;
						case QuoteProperty.DaysRangeRealtime:
							symbols.Append("m2");
							break;
						case QuoteProperty.FiftydayMovingAverage:
							symbols.Append("m3");
							break;
						case QuoteProperty.TwoHundreddayMovingAverage:
							symbols.Append("m4");
							break;
						case QuoteProperty.ChangeFromTwoHundreddayMovingAverage:
							symbols.Append("m5");
							break;
						case QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage:
							symbols.Append("m6");
							break;
						case QuoteProperty.ChangeFromFiftydayMovingAverage:
							symbols.Append("m7");
							break;
						case QuoteProperty.PercentChangeFromFiftydayMovingAverage:
							symbols.Append("m8");
							break;
						case QuoteProperty.Name:
							symbols.Append("n0");
							break;
						case QuoteProperty.Notes:
							symbols.Append("n4");
							break;
						case QuoteProperty.Open:
							symbols.Append("o0");
							break;
						case QuoteProperty.PreviousClose:
							symbols.Append("p0");
							break;
						case QuoteProperty.PricePaid:
							symbols.Append("p1");
							break;
						case QuoteProperty.ChangeInPercent:
							symbols.Append("p2");
							break;
						case QuoteProperty.PriceSales:
							symbols.Append("p5");
							break;
						case QuoteProperty.PriceBook:
							symbols.Append("p6");
							break;
						case QuoteProperty.ExDividendDate:
							symbols.Append("q0");
							break;
						case QuoteProperty.PERatio:
							symbols.Append("r0");
							break;
						case QuoteProperty.DividendPayDate:
							symbols.Append("r1");
							break;
						case QuoteProperty.PERatioRealtime:
							symbols.Append("r2");
							break;
						case QuoteProperty.PEGRatio:
							symbols.Append("r5");
							break;
						case QuoteProperty.PriceEPSEstimateCurrentYear:
							symbols.Append("r6");
							break;
						case QuoteProperty.PriceEPSEstimateNextYear:
							symbols.Append("r7");
							break;
						case QuoteProperty.Symbol:
							symbols.Append("s0");
							break;
						case QuoteProperty.SharesOwned:
							symbols.Append("s1");
							break;
						case QuoteProperty.Revenue:
							symbols.Append("s6");
							break;
						case QuoteProperty.ShortRatio:
							symbols.Append("s7");
							break;
						case QuoteProperty.LastTradeTime:
							symbols.Append("t1");
							break;
						case QuoteProperty.TradeLinks:
							symbols.Append("t6");
							break;
						case QuoteProperty.TickerTrend:
							symbols.Append("t7");
							break;
						case QuoteProperty.OneyrTargetPrice:
							symbols.Append("t8");
							break;
						case QuoteProperty.Volume:
							symbols.Append("v0");
							break;
						case QuoteProperty.HoldingsValue:
							symbols.Append("v1");
							break;
						case QuoteProperty.HoldingsValueRealtime:
							symbols.Append("v7");
							break;
						case QuoteProperty.YearRange:
							symbols.Append("w0");
							break;
						case QuoteProperty.DaysValueChange:
							symbols.Append("w1");
							break;
						case QuoteProperty.DaysValueChangeRealtime:
							symbols.Append("w4");
							break;
						case QuoteProperty.StockExchange:
							symbols.Append("x0");
							break;
						case QuoteProperty.TrailingAnnualDividendYieldInPercent:
							symbols.Append("y0");
							break;
					}
				}
			}
			return symbols.ToString();
		}
		public static char ServerToDelimiter(YahooServer server)
		{
			if (server == YahooServer.Australia | server == YahooServer.Canada | server == YahooServer.HongKong | server == YahooServer.India | server == YahooServer.Korea | server == YahooServer.Mexico | server == YahooServer.Singapore | server == YahooServer.UK | server == YahooServer.USA)
			{
				return ',';
			}
			else
			{
				return ';';
			}
		}
		private static string ReplaceDjiID(string id)
		{
			if (id.ToUpper() == "^DJI")
			{
				return "INDU";
			}
			else
			{
				return id;
			}
		}

		private FinanceHelper() { }

		public static class ImportExport
		{
			#region CSV


			/// <summary>
			/// Converts a list of quote data to a CSV formatted text
			/// </summary>
			/// <param name="quotes">The list of quote values</param>
			/// <param name="delimiter">The delimiter of the CSV text</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted data string in CSV format</returns>
			/// <remarks></remarks>
			public static string FromQuotesData(IEnumerable<YahooQuotesData> quotes, char delimiter, System.Globalization.CultureInfo culture = null)
			{
				return FromQuotesData(quotes, delimiter, null, culture);
			}
			/// <summary>
			/// Converts a list of quote data to a CSV formatted text
			/// </summary>
			/// <param name="quotes">The list of quote values</param>
			/// <param name="delimiter">The delimiter of the CSV text</param>
			/// <param name="properties">The used properties of the items</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted data string in CSV format</returns>
			/// <remarks></remarks>
			public static string FromQuotesData(IEnumerable<YahooQuotesData> quotes, char delimiter, IEnumerable<QuoteProperty> properties, System.Globalization.CultureInfo culture = null)
			{
				if (quotes != null)
				{
					System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
					if (culture != null)
						ci = culture;

					QuoteProperty[] prpts = FinanceHelper.CheckPropertiesOfQuotesData(quotes, properties);
					System.Text.StringBuilder sb = new System.Text.StringBuilder();

					foreach (QuoteProperty qp in prpts)
					{
						sb.Append(qp.ToString());
						sb.Append(delimiter);
					}
					sb.Remove(sb.Length - 1, 1);
					sb.AppendLine();

					foreach (YahooQuotesData q in quotes)
					{
						if (q != null)
						{
							System.Text.StringBuilder sbQ = new System.Text.StringBuilder();
							foreach (QuoteProperty qp in prpts)
							{
								object o = MyHelper.ObjectToString(q[qp], ci);
								if (o is string)
								{
									if (o.ToString() == string.Empty)
									{
										sbQ.Append("\"N/A\"");
									}
									else
									{
										sbQ.Append("\"");
										sbQ.Append(q[qp].ToString().Replace("\"", "\"\""));
										sbQ.Append("\"");
									}
								}
								else
								{
									sbQ.Append(MyHelper.ObjectToString(q[qp], ci));
								}
								sbQ.Append(delimiter);
							}
							if (sbQ.Length > 0)
								sbQ.Remove(sbQ.Length - 1, 1);
							sb.AppendLine(sbQ.ToString());
						}
					}
					return sb.ToString();
				}
				else
				{
					return string.Empty;
				}
			}
			/// <summary>
			/// Tries to read a list of quote data from a CSV formatted text (incl. Header)
			/// </summary>
			/// <param name="csvText">The CSV formatted text</param>
			/// <param name="delimiter">The delimiter of the CSV text</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted quote values or Nothing</returns>
			/// <remarks></remarks>
			public static YahooQuotesData[] ToQuotesData(string csvText, char delimiter, System.Globalization.CultureInfo culture = null)
			{
				List<QuoteProperty> properties = new List<QuoteProperty>();
				System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
				if (culture != null)
					ci = culture;

				if (csvText != string.Empty)
				{
					string[] rows = csvText.Split(new string[] {
				"\r",
				"\n"
			}, StringSplitOptions.RemoveEmptyEntries);
					string[] headerParts = MyHelper.CsvRowToStringArray(rows[0], delimiter);
					foreach (string part in headerParts)
					{
						foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
						{
							if (qp.ToString() == part.Trim())
							{
								properties.Add(qp);
								break; // TODO: might not be correct. Was : Exit For
							}
						}
					}
					if (properties.Count != headerParts.Length)
						return null;
				}

				return ToQuotesData(csvText, delimiter, properties.ToArray(), ci, true);
			}
			internal static YahooQuotesData[] ToQuotesData(string csvText, char delimiter, QuoteProperty[] properties, System.Globalization.CultureInfo culture, bool hasHeader = false)
			{
				List<YahooQuotesData> quotes = new List<YahooQuotesData>();
				if (csvText != string.Empty & culture != null & (properties != null && properties.Length > 0))
				{
					if (properties.Length > 0)
					{
						string[][] table = MyHelper.CsvTextToStringTable(csvText, delimiter);
						int start = 0;
						if (hasHeader)
							start = 1;

						if (table.Length > 0)
						{
							if (!(table[0].Length == properties.Length))
							{
								String[][] semicolTable = MyHelper.CsvTextToStringTable(csvText, Convert.ToChar((delimiter == ';' ? ',' : ';')));
								if (semicolTable[0].Length == properties.Length)
								{
									table = semicolTable;
								}
							}
							if (table.Length > 0 && table.Length > start)
							{
								for (int i = start; i <= table.Length - 1; i++)
								{
									YahooQuotesData qd = CsvArrayToQuoteData(table[i], properties, culture);
									if (qd != null)
										quotes.Add(qd);
								}
							}
						}
					}
				}
				return quotes.ToArray();
			}

			/// <summary>
			/// Converts a list of quote options to a CSV formatted text
			/// </summary>
			/// <param name="quoteOptions">The list of quote option values</param>
			/// <param name="delimiter">The delimiter of the CSV text</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted data string in CSV format</returns>
			/// <remarks></remarks>
			public static string FromQuoteOptions(IEnumerable<QuoteOptionsData> quoteOptions, char delimiter, System.Globalization.CultureInfo culture = null)
			{
				System.Text.StringBuilder csv = new System.Text.StringBuilder();
				if (quoteOptions != null)
				{
					System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
					if (culture != null)
						ci = culture;
					foreach (QuoteOptionsData qbd in quoteOptions)
					{
						csv.Append("\"");
						csv.Append(qbd.Symbol);
						csv.Append("\"");
						csv.Append("\"");
						csv.Append((qbd.Type == QuoteOptionType.Call ? "C" : "P").ToString());
						csv.Append("\"");
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(qbd.LastPrice, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(qbd.StrikePrice, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(Math.Abs(qbd.Change), ci));
						csv.Append(delimiter);
						csv.Append("\"");
						csv.Append((qbd.Change >= 0 ? "Up" : "Down").ToString());
						csv.Append("\"");
						csv.Append(MyHelper.ObjectToString(qbd.Bid, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(qbd.Ask, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(qbd.Volume, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(qbd.OpenInterest, ci));
					}
				}
				return csv.ToString();
			}
			/// <summary>
			/// Tries to read a list of quote options from a CSV formatted text
			/// </summary>
			/// <param name="csvText">The CSV formatted text</param>
			/// <param name="delimiter">The delimiter of the CSV text</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted quote values or Nothing</returns>
			/// <remarks></remarks>
			public static QuoteOptionsData[] ToQuoteOptions(string csvText, char delimiter, System.Globalization.CultureInfo culture = null)
			{
				List<QuoteOptionsData> lst = new List<QuoteOptionsData>();
				if (csvText != string.Empty)
				{
					System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
					if (culture != null)
						ci = culture;

					string[][] table = MyHelper.CsvTextToStringTable(csvText, delimiter);
					if (table.Length > 1)
					{
						for (int i = 0; i <= table.Length - 1; i++)
						{
							if (table[i].Length == 10)
							{
								QuoteOptionsData qd = new QuoteOptionsData();
								qd.Symbol = table[i][0];
								qd.Type = (QuoteOptionType)(table[i][1].ToLower() == "p" ? QuoteOptionType.Put : QuoteOptionType.Call);
								double t1;
								double t2;
								double t3;
								double t4;
								double t5;
								int t6;
								int t7;
								if (double.TryParse(table[i][2], System.Globalization.NumberStyles.Currency, ci, out t1) &&
									double.TryParse(table[i][3], System.Globalization.NumberStyles.Currency, ci, out t2) &&
									double.TryParse(table[i][4], System.Globalization.NumberStyles.Currency, ci, out t3) &&
									double.TryParse(table[i][6], System.Globalization.NumberStyles.Currency, ci, out t4) &&
									double.TryParse(table[i][7], System.Globalization.NumberStyles.Currency, ci, out t5) &&
									int.TryParse(table[i][8], System.Globalization.NumberStyles.Integer, ci, out t6) &&
									int.TryParse(table[i][9], System.Globalization.NumberStyles.Integer, ci, out t7))
								{
									qd.LastPrice = t1;
									qd.StrikePrice = t2;
									qd.Change = t3;
									qd.Bid = t4;
									qd.Ask = t5;
									qd.Volume = t6;
									qd.OpenInterest = t7;

									qd.Change *= Convert.ToInt32((table[i][5].ToLower() == "down" ? -1 : 1));
									lst.Add(qd);
								}
							}
						}
					}
				}
				return lst.ToArray();
			}

			/// <summary>
			/// Converts a list of historic quote periods to a CSV formatted text
			/// </summary>
			/// <param name="quotes">The list of historic quote periods</param>
			/// <param name="delimiter">The delimiter of the CSV text</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted data string in CSV format</returns>
			/// <remarks></remarks>
			public static string FromHistQuotesData(IEnumerable<HistQuotesData> quotes, char delimiter, System.Globalization.CultureInfo culture = null)
			{
				System.Text.StringBuilder csv = new System.Text.StringBuilder();
				if (quotes != null)
				{
					System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
					if (culture != null)
						ci = culture;
					csv.AppendLine(HistQuotesCSVHeadline(delimiter));
					foreach (HistQuotesData hq in quotes)
					{
						csv.Append(MyHelper.ObjectToString(hq.TradingDate, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(hq.Open, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(hq.High, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(hq.Low, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(hq.Close, ci));
						csv.Append(delimiter);
						csv.Append(MyHelper.ObjectToString(hq.Volume, ci));
						csv.Append(delimiter);
						csv.AppendLine(MyHelper.ObjectToString(hq.CloseAdjusted, ci));
					}
				}
				return csv.ToString();
			}
			/// <summary>
			/// Tries to read a list of historic quote periods from a CSV formatted text
			/// </summary>
			/// <param name="csvText">The CSV formatted text</param>
			/// <param name="delimiter">The delimiter of the CSV text</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted historic quote periods or Nothing</returns>
			/// <remarks></remarks>
			public static HistQuotesData[] ToHistQuotesData(string csvText, char delimiter, System.Globalization.CultureInfo culture = null)
			{
				List<HistQuotesData> lst = new List<HistQuotesData>();
				if (csvText != string.Empty)
				{
					System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
					if (culture != null)
						ci = culture;
					string[][] table = MyHelper.CsvTextToStringTable(csvText, delimiter);
					if (table.Length > 1)
					{
						for (int i = 0; i <= table.Length - 1; i++)
						{
							if (table[i].Length == 7)
							{
								HistQuotesData qd = new HistQuotesData();
								System.DateTime t1;
								double t2;
								double t3;
								double t4;
								double t5;
								double t6;
								long t7;

								if (System.DateTime.TryParse(table[i][0], culture, System.Globalization.DateTimeStyles.None, out t1) &&
									double.TryParse(table[i][1], System.Globalization.NumberStyles.Currency, culture, out t2) &&
									double.TryParse(table[i][2], System.Globalization.NumberStyles.Currency, culture, out t3) &&
									double.TryParse(table[i][3], System.Globalization.NumberStyles.Currency, culture, out t4) &&
									double.TryParse(table[i][4], System.Globalization.NumberStyles.Currency, culture, out t5) &&
									double.TryParse(table[i][6], System.Globalization.NumberStyles.Currency, culture, out t6) &&
									long.TryParse(table[i][5], System.Globalization.NumberStyles.Integer, culture, out t7))
								{
									qd.TradingDate = t1;
									qd.Open = t2;
									qd.High = t3;
									qd.Low = t4;
									qd.Close = t5;
									qd.CloseAdjusted = t6;
									qd.Volume = t7;

									lst.Add(qd);
								}
							}
						}
					}
				}
				return lst.ToArray();
			}

			public static MarketQuotesData[] ToMarketQuotesData(string csv, System.Globalization.CultureInfo culture = null)
			{
				System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
				if (culture != null)
					ci = culture;
				char delimiter = ',';
				string[][] table = MyHelper.CsvTextToStringTable(csv, delimiter);
				List<MarketQuotesData> lst = new List<MarketQuotesData>();
				if (table.Length > 1)
				{
					for (int i = 1; i <= table.Length - 1; i++)
					{
						if (table[i].Length == 10)
						{
							MarketQuotesData quote = new MarketQuotesData();
							quote.Name = table[i][0];

							double t1;
							double t2;
							double t3;
							double t4;
							double t5;
							double t6;
							double t7;
							double t8;

							if (double.TryParse(table[i][1], System.Globalization.NumberStyles.Any, ci, out t1))
								quote.OneDayPriceChangePercent = t1;
							string mktcap = table[i][2];
							if (mktcap != "NA" & mktcap != string.Empty & mktcap.Length > 1)
							{
								double value = 0;
								double.TryParse(mktcap.Substring(0, mktcap.Length - 1), System.Globalization.NumberStyles.Any, ci, out value);
								quote.MarketCapitalizationInMillion = value * FinanceHelper.GetStringMillionFactor(mktcap);
							}
							if (double.TryParse(table[i][3], System.Globalization.NumberStyles.Any, ci, out t2)) quote.PriceEarningsRatio = t2;
							if (double.TryParse(table[i][4], System.Globalization.NumberStyles.Any, ci, out t3)) quote.ReturnOnEquityPercent = t3;
							if (double.TryParse(table[i][5], System.Globalization.NumberStyles.Any, ci, out t4)) quote.DividendYieldPercent = t4;
							if (double.TryParse(table[i][6], System.Globalization.NumberStyles.Any, ci, out t5)) quote.LongTermDeptToEquity = t5;
							if (double.TryParse(table[i][7], System.Globalization.NumberStyles.Any, ci, out t6)) quote.PriceToBookValue = t6;
							if (double.TryParse(table[i][8], System.Globalization.NumberStyles.Any, ci, out t7)) quote.NetProfitMarginPercent = t7;
							if (double.TryParse(table[i][9], System.Globalization.NumberStyles.Any, ci, out t8)) quote.PriceToFreeCashFlow = t8;
							lst.Add(quote);
						}
					}
				}
				return lst.ToArray();
			}


			private static string HistQuotesCSVHeadline(char delimiter)
			{
				return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}Adj Close", delimiter, FinanceHelper.NameHistQuoteDate, FinanceHelper.NameHistQuoteOpen, FinanceHelper.NameHistQuoteHigh, FinanceHelper.NameHistQuoteLow, FinanceHelper.NameHistQuoteClose, FinanceHelper.NameHistQuoteVolume);
			}


			private static object QuoteStringToObject(string value, QuoteProperty prp, System.Globalization.CultureInfo culture)
			{
				object o = MyHelper.StringToObject(value, culture);
				if (prp == QuoteProperty.Name && o is Array && ((Array)o).Length > 0)
				{
					Array arr = (Array)o;
					string s = "";
					for (int i = 0; i < arr.Length; i++)
					{
						s += arr.GetValue(i).ToString();
						if (i != arr.Length - 1)
						{
							s += " - ";
						}
					}
					return s;
				}
				else
				{
					return o;
				}
			}


			private static YahooQuotesData CsvArrayToQuoteData(string[] rowItems, QuoteProperty[] properties, System.Globalization.CultureInfo culture)
			{
				if (rowItems.Length > 0)
				{
					YahooQuotesData quote = null;
					if (rowItems.Length == properties.Length)
					{
						quote = new YahooQuotesData();
						for (int i = 0; i <= properties.Length - 1; i++)
						{
							quote[properties[i]] = QuoteStringToObject(rowItems[i], properties[i], culture);
						}
					}
					else
					{

						if (rowItems.Length > 1)
						{
							List<QuoteProperty> alternateProperties = new List<QuoteProperty>();
							foreach (QuoteProperty qp in properties)
							{
								foreach (QuoteProperty q in mAlternateQuoteProperties)
								{
									if (qp == q)
									{
										alternateProperties.Add(qp);
										break;
									}
								}
							}


							if (alternateProperties.Count > 0)
							{
								List<KeyValuePair<QuoteProperty, int>[]> lst = new List<KeyValuePair<QuoteProperty, int>[]>();
								int[][] permutations = MaxThreePerm(alternateProperties.Count, Math.Min(rowItems.Length - properties.Length + 1, 3));
								foreach (int[] perm in permutations)
								{
									List<KeyValuePair<QuoteProperty, int>> lst2 = new List<KeyValuePair<QuoteProperty, int>>();
									for (int i = 0; i <= alternateProperties.Count - 1; i++)
									{
										lst2.Add(new KeyValuePair<QuoteProperty, int>(alternateProperties[i], perm[i]));
									}
									lst.Add(lst2.ToArray());
								}

								foreach (KeyValuePair<QuoteProperty, int>[] combination in lst)
								{
									String[] newRowItems = CsvNewRowItems(rowItems, properties, combination);

									try
									{
										if (newRowItems.Length > 0)
										{
											quote = new YahooQuotesData();
											for (int i = 0; i <= properties.Length - 1; i++)
											{
												quote[properties[i]] = QuoteStringToObject(rowItems[i], properties[i], culture);
											}
											break;
										}
									}
									catch (Exception ex)
									{
										System.Diagnostics.Debug.WriteLine(ex.Message);
									}
								}

							}
						}
					}
					return quote;
				}
				else
				{
					return null;
				}
			}


			public static int[][] MaxThreePerm(int propertyCount, int maxCount)
			{
				List<int[]> lst = new List<int[]>();
				for (int i = 1; i <= maxCount; i++)
				{
					if (propertyCount > 1)
					{
						for (int n = 1; n <= maxCount; n++)
						{
							if (propertyCount > 2)
							{
								for (int m = 1; m <= maxCount; m++)
								{
									lst.Add(new int[] {
								i,
								n,
								m
							});
								}
							}
							else
							{
								lst.Add(new int[] {
							i,
							n
						});
							}
						}
					}
					else
					{
						lst.Add(new int[] { i });
					}
				}
				return lst.ToArray();
			}


			private static QuoteProperty[] mAlternateQuoteProperties = new QuoteProperty[] {
		QuoteProperty.LastTradeSize,
		QuoteProperty.BidSize,
		QuoteProperty.AskSize

	};

			private static string[] CsvNewRowItems(string[] oldItems, QuoteProperty[] properties, KeyValuePair<QuoteProperty, int>[] multipleItemProperties)
			{
				System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
				List<string> newRowItems = new List<string>();
				int itemsCount = properties.Length;
				foreach (KeyValuePair<QuoteProperty, int> q in multipleItemProperties)
				{
					itemsCount += q.Value - 1;
				}
				if (itemsCount == oldItems.Length)
				{
					int actualIndex = 0;
					foreach (QuoteProperty qp in properties)
					{
						Nullable<KeyValuePair<QuoteProperty, int>> alternatProperty = null;
						foreach (KeyValuePair<QuoteProperty, int> q in multipleItemProperties)
						{
							if (q.Key == qp)
							{
								alternatProperty = q;
								break; // TODO: might not be correct. Was : Exit For
							}
						}
						if (!alternatProperty.HasValue)
						{
							newRowItems.Add(oldItems[actualIndex]);
						}
						else
						{
							string newRowItem = string.Empty;

							for (int i = actualIndex; i <= (actualIndex + alternatProperty.Value.Value - 1); i++)
							{
								int @int = 0;
								if (int.TryParse(oldItems[i], System.Globalization.NumberStyles.Integer, convCulture, out @int) && (oldItems[i] == @int.ToString() || oldItems[i] == "000"))
								{
									newRowItem += oldItems[i];
								}
								else
								{
									newRowItem = string.Empty;
									break; // TODO: might not be correct. Was : Exit For
								}
							}
							if (newRowItem != string.Empty)
							{
								newRowItems.Add(newRowItem);
								actualIndex += alternatProperty.Value.Value - 1;
							}
						}
						actualIndex += 1;
					}
				}
				return newRowItems.ToArray();
			}



			#endregion

			#region XML

			/// <summary>
			/// Writes a QuoteData to XML format
			/// </summary>
			/// <param name="writer">The used XML writer</param>
			/// <param name="quote">The used QuoteData</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <remarks></remarks>
			public static void FromQuoteData(System.Xml.XmlWriter writer, YahooQuotesData quote, System.Globalization.CultureInfo culture = null)
			{
				FromQuoteData(writer, quote, null, culture);
			}
			/// <summary>
			/// Writes a QuoteData to XML format
			/// </summary>
			/// <param name="writer">The used XML writer</param>
			/// <param name="quote">The used QuoteData</param>
			/// <param name="properties">The used properties of the quotes</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <remarks></remarks>
			public static void FromQuoteData(System.Xml.XmlWriter writer, YahooQuotesData quote, IEnumerable<QuoteProperty> properties, System.Globalization.CultureInfo culture = null)
			{
				System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
				if (culture != null)
					ci = culture;
				writer.WriteStartElement("Quote");
				if (quote[QuoteProperty.Symbol] != null)
					writer.WriteAttributeString("ID", quote[QuoteProperty.Symbol].ToString());
				QuoteProperty[] prps = FinanceHelper.CheckPropertiesOfQuotesData(new YahooQuotesData[] { quote }, properties);
				foreach (QuoteProperty qp in prps)
				{
					writer.WriteStartElement(qp.ToString());
					writer.WriteValue(MyHelper.ObjectToString(quote[qp], ci));
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			/// <summary>
			/// Tries to read a QuoteData from XML
			/// </summary>
			/// <param name="node">The XML node of a QuoteData</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted quote data or Nothing</returns>
			/// <remarks></remarks>
			public static YahooQuotesData ToQuoteData(XParseElement node, System.Globalization.CultureInfo culture = null)
			{
				if (node != null && node.Name.LocalName.ToLower() == "quote")
				{
					System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
					if (culture != null)
						ci = culture;
					YahooQuotesData quote = new YahooQuotesData();
					foreach (XParseElement propertyNode in node.Elements())
					{
						foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
						{
							if (propertyNode.Name.LocalName == qp.ToString())
							{
								quote[qp] = MyHelper.StringToObject(propertyNode.Value, ci);
								break; // TODO: might not be correct. Was : Exit For
							}
						}
					}
					return quote;
				}
				else
				{
					return null;
				}
			}

			/// <summary>
			/// Writes a QuoteOption to XML format
			/// </summary>
			/// <param name="writer">The used XML writer</param>
			/// <param name="quoteOption">The used QuoteOption</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <remarks></remarks>
			public static void FromQuoteOption(System.Xml.XmlWriter writer, QuoteOptionsData quoteOption, System.Globalization.CultureInfo culture = null)
			{
				System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
				if (culture != null)
					ci = culture;
				writer.WriteStartElement("Option");

				writer.WriteAttributeString(FinanceHelper.NameOptionSymbol, quoteOption.Symbol);
				writer.WriteAttributeString(FinanceHelper.NameOptionType, (quoteOption.Type == QuoteOptionType.Call ? "C" : "P").ToString());

				writer.WriteStartElement(FinanceHelper.NameOptionLastPrice);
				writer.WriteValue(MyHelper.ObjectToString(quoteOption.LastPrice, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameOptionStrikePrice);
				writer.WriteValue(MyHelper.ObjectToString(quoteOption.StrikePrice, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameOptionChange);
				writer.WriteValue(MyHelper.ObjectToString(Math.Abs(quoteOption.Change), ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameOptionChangeDir);
				writer.WriteValue((quoteOption.Change >= 0 ? "Up" : "Down").ToString());
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameOptionBid);
				writer.WriteValue(MyHelper.ObjectToString(quoteOption.Bid, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameOptionAsk);
				writer.WriteValue(MyHelper.ObjectToString(quoteOption.Ask, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameOptionVolume);
				writer.WriteValue(MyHelper.ObjectToString(quoteOption.Volume, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameOptionOpenInterest);
				writer.WriteValue(MyHelper.ObjectToString(quoteOption.OpenInterest, ci));
				writer.WriteEndElement();

				writer.WriteEndElement();
			}
			/// <summary>
			/// Tries to read a QuoteOption from XML
			/// </summary>
			/// <param name="node">The XML node of QuoteOption</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted quote data or Nothing</returns>
			/// <remarks></remarks>
			public static QuoteOptionsData ToQuoteOption(XParseElement node, System.Globalization.CultureInfo culture = null)
			{
				if (node != null && node.Name.LocalName.ToLower() == "option")
				{
					System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
					if (culture != null)
						ci = culture;
					string symbol = string.Empty;
					string t = string.Empty;
					foreach (XParseAttribute att in node.Attributes())
					{
						switch (att.Name.LocalName)
						{
							case FinanceHelper.NameOptionSymbol:
								symbol = att.Value;
								break;
							case "type":
								t = att.Value;
								break;
						}
					}
					QuoteOptionType type = QuoteOptionType.Call;
					if (t.ToLower() == "p")
						type = QuoteOptionType.Put;
					double strikePrice = 0;
					double lastPrice = 0;
					double change = 0;
					double bid = 0;
					double ask = 0;
					int volume = 0;
					int openInt = 0;
					foreach (XParseElement propertyNode in node.Elements())
					{
						switch (propertyNode.Name.LocalName)
						{
							case FinanceHelper.NameOptionStrikePrice:
								double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out strikePrice);
								break;
							case FinanceHelper.NameOptionLastPrice:
								double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out lastPrice);
								break;
							case FinanceHelper.NameOptionChange:
								double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out change);
								break;
							case FinanceHelper.NameOptionChangeDir:
								if (propertyNode.Value == "Down")
									change *= -1;
								break;
							case FinanceHelper.NameOptionBid:
								double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out bid);
								break;
							case FinanceHelper.NameOptionAsk:
								double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out ask);
								break;
							case FinanceHelper.NameOptionVolume:
								int.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Integer, ci, out volume);
								break;
							case FinanceHelper.NameOptionOpenInterest:
								int.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Integer, ci, out openInt);
								break;
						}
					}
					return new QuoteOptionsData(symbol, type, strikePrice, lastPrice, change, bid, ask, volume, openInt);
				}
				else
				{
					return null;
				}
			}

			/// <summary>
			/// Writes HistQuoteData to XML format
			/// </summary>
			/// <param name="writer">The used XML writer</param>
			/// <param name="quote">The used HistQuoteData</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <remarks>Creates a main node for all periods</remarks>
			public static void FromHistQuoteData(System.Xml.XmlWriter writer, HistQuotesData quote, System.Globalization.CultureInfo culture = null)
			{
				System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
				if (culture != null)
					ci = culture;
				writer.WriteStartElement("HistQuote");

				writer.WriteStartElement(FinanceHelper.NameHistQuoteDate);
				writer.WriteValue(quote.TradingDate.ToShortDateString());
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameHistQuoteOpen);
				writer.WriteValue(MyHelper.ObjectToString(quote.Open, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameHistQuoteHigh);
				writer.WriteValue(MyHelper.ObjectToString(quote.High, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameHistQuoteLow);
				writer.WriteValue(MyHelper.ObjectToString(quote.Low, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameHistQuoteClose);
				writer.WriteValue(MyHelper.ObjectToString(quote.Close, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameHistQuoteVolume);
				writer.WriteValue(MyHelper.ObjectToString(quote.Volume, ci));
				writer.WriteEndElement();

				writer.WriteStartElement(FinanceHelper.NameHistQuoteAdjClose);
				writer.WriteValue(MyHelper.ObjectToString(quote.CloseAdjusted, ci));
				writer.WriteEndElement();

				writer.WriteEndElement();
			}
			/// <summary>
			/// Tries to read a HistQuoteData from XML
			/// </summary>
			/// <param name="node">The XML node of HistQuoteData</param>
			/// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
			/// <returns>The converted historic quote data or Nothing</returns>
			/// <remarks></remarks>
			public static HistQuotesData ToHistQuoteData(XParseElement node, System.Globalization.CultureInfo culture = null)
			{
				if (node != null && node.Name.LocalName.ToLower() == "histquote" && MyHelper.EnumToArray(node.Elements()).Length > 0)
				{
					System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
					if (culture != null)
						ci = culture;
					XParseElement[] elm = MyHelper.EnumToArray(node.Elements());
					HistQuotesData qd = new HistQuotesData();
					foreach (XParseElement cNode in node.Elements())
					{
						switch (cNode.Name.LocalName)
						{
							case FinanceHelper.NameHistQuoteDate:
								System.DateTime t1;
								if (System.DateTime.TryParse(elm[0].Value, out t1)) qd.TradingDate = t1;
								break;
							case FinanceHelper.NameHistQuoteOpen:
								double t2;
								if (double.TryParse(elm[1].Value, System.Globalization.NumberStyles.Currency, ci, out t2)) qd.Open = t2;
								break;
							case FinanceHelper.NameHistQuoteHigh:
								double t3;
								if (double.TryParse(elm[2].Value, System.Globalization.NumberStyles.Currency, ci, out t3)) qd.High = t3;
								break;
							case FinanceHelper.NameHistQuoteLow:
								double t4;
								if (double.TryParse(elm[3].Value, System.Globalization.NumberStyles.Currency, ci, out t4)) qd.Low = t4;
								break;
							case FinanceHelper.NameHistQuoteClose:
								double t5;
								if (double.TryParse(elm[4].Value, System.Globalization.NumberStyles.Currency, ci, out t5)) qd.Close = t5;
								break;
							case FinanceHelper.NameHistQuoteAdjClose:
								double t6;
								if (double.TryParse(elm[6].Value, System.Globalization.NumberStyles.Currency, ci, out t6)) qd.CloseAdjusted = t6;
								break;
							case FinanceHelper.NameHistQuoteVolume:
								long t7;
								if (long.TryParse(elm[5].Value, System.Globalization.NumberStyles.Integer, ci, out t7)) qd.Volume = t7;
								break;
						}
					}
					return qd;
				}
				else
				{
					return null;
				}
			}




			#endregion

		}
	}
}
