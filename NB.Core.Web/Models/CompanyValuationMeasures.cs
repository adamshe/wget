using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class CompanyValuationMeasures
    {
        /// <summary>
        /// The total dollar value of all outstanding shares. Computed as shares times current market price. Capitalization is a measure of corporate size.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price Per Share x Number of Shares Outstanding
        /// Intraday Value
        /// Shares outstanding is taken from the most recently filed quarterly or annual report and Market Cap is calculated using shares outstanding.</remarks>
        public double MarketCapitalisationInMillion { get; set; }
        /// <summary>
        /// Enterprise Value is a measure of theoretical takeover price, and is useful in comparisons against income statement line items above the interest expense/income lines such as revenue and EBITDA.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Market Cap + Total Debt - Total Cash &amp; Short Term Investments</remarks>
        public double EnterpriseValueInMillion { get; set; }
        /// <summary>
        /// A popular valuation ratio calculated by dividing the current market price by trailing 12-month (ttm) Earnings Per Share.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Earnings Per Share
        /// Intraday Value
        /// Trailing Twelve Months</remarks>
        public double TrailingPE { get; set; }
        /// <summary>
        /// A valuation ratio calculated by dividing the current market price by projected 12-month Earnings Per Share.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Projected Earnings Per Share
        /// Fiscal Year Ending</remarks>
        public double ForwardPE { get; set; }
        /// <summary>
        /// Forward-looking measure rather than typical earnings growth measures, which look eck in time (historical). Used to measure a stock's valuation against its projected 5-yr growth rate.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: P/E Ratio / 5-Yr Expected EPS Growth
        /// 5 years expected</remarks>
        public double PEGRatio { get; set; }
        /// <summary>
        /// A valuation ratio calculated by dividing the current market price by trailing 12-month (ttm) Total Revenues. Often used to value unprofitable companies.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Total Revenues Per Share
        /// Trailing Twelve Months</remarks>
        public double PriceToSales { get; set; }
        /// <summary>
        /// A valuation ratio calculated by dividing the current market price by the most recent quarter's (mrq) Book Value Per Share.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Book Value Per Share
        /// Most Recent Quarter</remarks>
        public double PriceToBook { get; set; }
        /// <summary>
        /// Firm value compared against revenue. Provides a more rigorous comparison than the Price/Sales ratio by removing the effects of capitalization from both sides of the ratio. Since revenue is unaffected by the interest income/expense line item, the appropriate value comparison should also remove the effects of capitalization, as EV does.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Enterprise Value / Total Revenues
        /// Trailing Twelve Months</remarks>
        public double EnterpriseValueToRevenue { get; set; }
        /// <summary>
        /// Firm value compared against EBITDA (Earnings before interest, taxes, depreciation, and amortization). See Enterprise Value/Revenue.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Enterprise Value / EBITDA
        /// Trailing Twelve Months</remarks>
        public double EnterpriseValueToEBITDA { get; set; }

        internal CompanyValuationMeasures(double[] values)
        {
            this.MarketCapitalisationInMillion = values[0];
            this.EnterpriseValueInMillion = values[1];
            this.TrailingPE = values[2];
            this.ForwardPE = values[3];
            this.PEGRatio = values[4];
            this.PriceToSales = values[5];
            this.PriceToBook = values[6];
            this.EnterpriseValueToRevenue = values[7];
            this.EnterpriseValueToEBITDA = values[8];
        }
    }
}
