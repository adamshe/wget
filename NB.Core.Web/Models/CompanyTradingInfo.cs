using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class CompanyTradingInfo
    {

        //StockPriceHistory
        /// <summary>
        /// The Beta used is Beta of Equity. Beta is the monthly price change of a particular company relative to the monthly price change of the S&amp;P500. The time period for Beta is 3 years (36 months) when available.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Beta { get; set; }
        /// <summary>
        /// The percentage change in price from 52 weeks ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double OneYearChangePercent { get; set; }
        /// <summary>
        /// The S&amp;P 500 Index's percentage change in price from 52 weeks ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double SP500OneYearChangePercent { get; set; }
        /// <summary>
        /// This price is the highest Price the stock traded at in the last 12 months. This could be an intraday high.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double OneYearHigh { get; set; }
        /// <summary>
        /// This price is the lowest Price the stock traded at in the last 12 months. This could be an intraday low.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double OneYearLow { get; set; }
        /// <summary>
        /// A simple moving average that is calculated by dividing the sum of the closing prices in the last 50 trading days by 50.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double FiftyDayMovingAverage { get; set; }
        /// <summary>
        /// A simple moving average that is calculated by dividing the sum of the closing prices in the last 200 trading days by 200.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TwoHundredDayMovingAverage { get; set; }

        //Share Statistics
        /// <summary>
        /// This is the average daily trading volume during the last 3 months.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double AverageVolumeThreeMonthInThousand { get; set; }
        /// <summary>
        /// This is the average daily trading volume during the last 10 days.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double AverageVolumeTenDaysInThousand { get; set; }
        /// <summary>
        /// This is the number of shares of common stock currently outstanding—the number of shares issued minus the shares held in treasury. This _institutionalOwnerShip reflects all offerings and acquisitions for stock made after the end of the previous fiscal period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double SharesOutstandingInMillion { get; set; }
        /// <summary>
        /// This is the number of freely traded shares in the hands of the public. Float is calculated as Shares Outstanding minus Shares Owned by Insiders, 5% Owners, and Rule 144 Shares.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double FloatInMillion { get; set; }
        /// <summary>
        /// This is the number of shares currently borrowed by investors for sale, but not yet returned to the owner (lender).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double PercentHeldByInsiders { get; set; }
        public double PercentHeldByInstitutions { get; set; }
        public double SharesShortInMillion { get; set; }
        /// <summary>
        /// This represents the number of days it would take to cover the Short Interest if trading continued at the average daily volume for the month. It is calculated as the Short Interest for the Current Month divided by the Average Daily Volume.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ShortRatio { get; set; }
        /// <summary>
        /// Number of shares short divided by float.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ShortPercentOfFloat { get; set; }
        /// <summary>
        /// Shares Short in the prior month. See Shares Short.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double SharesShortPriorMonthInMillion { get; set; }


        //Dividends and Splits
        /// <summary>
        /// The annualized amount of dividends expected to be paid in the current fiscal year.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ForwardAnnualDividendRate { get; set; }
        /// <summary>
        /// Formula: (Forward Annual Dividend Rate / Current Market Price) x 100
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ForwardAnnualDividendYieldPercent { get; set; }
        /// <summary>
        /// The sum of all dividends paid out in the trailing 12-month period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TrailingAnnualDividendYield { get; set; }
        /// <summary>
        /// Formula: (Trailing Annual Dividend Rate / Current Market Price) x 100
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TrailingAnnualDividendYieldPercent { get; set; }
        /// <summary>
        /// The average Forward Annual Dividend Yield in the past 5 years.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double FiveYearAverageDividendYieldPercent { get; set; }
        /// <summary>
        /// The ratio of Earnings paid out in Dividends, expressed as a percentage.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double PayoutRatio { get; set; }
        /// <summary>
        /// The payment date for a declared dividend.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime DividendDate { get; set; }
        /// <summary>
        /// The first day of trading when the seller, rather than the buyer, of a stock is entitled to the most recently announced dividend payment. The date set by the NYSE (and generally followed on other U.S. exchanges) is currently two business days before the record date. A stock that has gone ex-dividend is denoted by an x in newspaper listings on that date.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime ExDividendDate { get; set; }
        public System.DateTime LastSplitDate { get; set; }
        public SharesSplitFactor LastSplitFactor { get; set; }



        internal CompanyTradingInfo(double[] values, System.DateTime dividendDate, System.DateTime exDividendDate, System.DateTime lastSplitDate, SharesSplitFactor lastSplitFactor)
        {
            this.Beta = values[0];
            this.OneYearChangePercent = values[1];
            this.SP500OneYearChangePercent = values[2];
            this.OneYearHigh = values[3];
            this.OneYearLow = values[4];
            this.FiftyDayMovingAverage = values[5];
            this.TwoHundredDayMovingAverage = values[6];

            this.AverageVolumeThreeMonthInThousand = values[7];
            this.AverageVolumeTenDaysInThousand = values[8];
            this.SharesOutstandingInMillion = values[9];
            this.FloatInMillion = values[10];
            this.PercentHeldByInsiders = values[11];
            this.PercentHeldByInstitutions = values[12];
            this.SharesShortInMillion = values[13];
            this.ShortRatio = values[14];
            this.ShortPercentOfFloat = values[15];
            this.SharesShortPriorMonthInMillion = values[16];

            this.ForwardAnnualDividendRate = values[17];
            this.ForwardAnnualDividendYieldPercent = values[18];
            this.TrailingAnnualDividendYield = values[19];
            this.TrailingAnnualDividendYieldPercent = values[20];
            this.FiveYearAverageDividendYieldPercent = values[21];
            this.PayoutRatio = values[22];
            this.DividendDate = dividendDate;
            this.ExDividendDate = exDividendDate;
            this.LastSplitFactor = lastSplitFactor;
            this.LastSplitDate = lastSplitDate;
        }

    }
}
