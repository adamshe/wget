using NB.Core.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class MarketQuotesData
    {

        /// <summary>
        /// The name of the sector, industry or company.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name { get; set; }
        public double OneDayPriceChangePercent { get; set; }
        public double MarketCapitalizationInMillion { get; set; }
        public double PriceEarningsRatio { get; set; }
        public double ReturnOnEquityPercent { get; set; }
        public double DividendYieldPercent { get; set; }
        public double LongTermDeptToEquity { get; set; }
        public double PriceToBookValue { get; set; }
        public double NetProfitMarginPercent { get; set; }
        public double PriceToFreeCashFlow { get; set; }

        /// <summary>
        /// All market quote properties.
        /// </summary>
        /// <param name="prp">The market quote property you want to get or set.</param>
        /// <value></value>
        /// <returns>A value representing and depending on the passed property.</returns>
        /// <remarks></remarks>
        public object this[MarketQuoteProperty prp]
        {
            get
            {
                switch (prp)
                {
                    case MarketQuoteProperty.Name:
                        return this.Name;
                    case MarketQuoteProperty.DividendYieldPercent:
                        return this.DividendYieldPercent;
                    case MarketQuoteProperty.LongTermDeptToEquity:
                        return this.LongTermDeptToEquity;
                    case MarketQuoteProperty.MarketCapitalizationInMillion:
                        return this.MarketCapitalizationInMillion;
                    case MarketQuoteProperty.NetProfitMarginPercent:
                        return this.NetProfitMarginPercent;
                    case MarketQuoteProperty.OneDayPriceChangePercent:
                        return this.OneDayPriceChangePercent;
                    case MarketQuoteProperty.PriceEarningsRatio:
                        return this.PriceEarningsRatio;
                    case MarketQuoteProperty.PriceToBookValue:
                        return this.PriceToBookValue;
                    case MarketQuoteProperty.PriceToFreeCashFlow:
                        return this.PriceToFreeCashFlow;
                    case MarketQuoteProperty.ReturnOnEquityPercent:
                        return this.ReturnOnEquityPercent;
                    default:
                        return null;
                }
            }
            set
            {
                switch (prp)
                {
                    case MarketQuoteProperty.Name:
                        this.Name = value.ToString();
                        break;
                    case MarketQuoteProperty.DividendYieldPercent:
                        double t1;
                        if (double.TryParse(value.ToString(), out t1))
                            this.DividendYieldPercent = t1;
                        break;
                    case MarketQuoteProperty.LongTermDeptToEquity:
                        double t2;
                        if (double.TryParse(value.ToString(), out t2))
                            this.LongTermDeptToEquity = t2;
                        break;
                    case MarketQuoteProperty.MarketCapitalizationInMillion:
                        double t3;
                        if (double.TryParse(value.ToString(), out t3))
                            this.MarketCapitalizationInMillion = t3;
                        break;
                    case MarketQuoteProperty.NetProfitMarginPercent:
                        double t4;
                        if (double.TryParse(value.ToString(), out t4))
                            this.NetProfitMarginPercent = t4;
                        break;
                    case MarketQuoteProperty.OneDayPriceChangePercent:
                        double t5;
                        if (double.TryParse(value.ToString(), out t5))
                            this.OneDayPriceChangePercent = t5;
                        break;
                    case MarketQuoteProperty.PriceEarningsRatio:
                        double t6;
                        if (double.TryParse(value.ToString(), out t6))
                            this.PriceEarningsRatio = t6;
                        break;
                    case MarketQuoteProperty.PriceToBookValue:
                        double t7;
                        if (double.TryParse(value.ToString(), out t7))
                            this.PriceToBookValue = t7;
                        break;
                    case MarketQuoteProperty.PriceToFreeCashFlow:
                        double t8;
                        if (double.TryParse(value.ToString(), out t8))
                            this.PriceToFreeCashFlow = t8;
                        break;
                    case MarketQuoteProperty.ReturnOnEquityPercent:
                        double t9;
                        if (double.TryParse(value.ToString(), out t9))
                            this.ReturnOnEquityPercent = t9;
                        break;
                }
            }
        }

    }
}
