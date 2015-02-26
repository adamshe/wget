using NB.Core.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class QuoteOptionsData
    {

        private double[] mValues = new double[5];
        /// <summary>
        /// The basic parts of new option symbol are: Root symbol + Expiration Year(yy)+ Expiration Month(mm)+ Expiration Day(dd) + Call/Put Indicator (C or P) + Strike price
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Symbol { get; set; }
        /// <summary>
        /// Call/Put Indicator
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public QuoteOptionType Type { get; set; }
        /// <summary>
        ///  The stated price per share for which underlying stock can be purchased (in the case of a call) or sold (in the case of a put) by the option holder upon exercise of the option contract.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double StrikePrice { get; set; }
        /// <summary>
        /// The price of the last trade made for option contract.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double LastPrice { get; set; }
        /// <summary>
        /// The change in price for the day. This is the difference between the last trade and the previous day's closing price (Prev Close). The change is reported as "0" if the option hasn't traded today.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Change { get; set; }
        /// <summary>
        /// The Bid price is the price you get if you were to write (sell) an option.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Bid { get; set; }
        /// <summary>
        /// The Ask price is the price you have to pay to purchase an option.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Ask { get; set; }
        /// <summary>
        /// The volume indicates the number of option contracts that have traded for the current day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Volume { get; set; }
        /// <summary>
        /// The total number of derivative contracts traded that have not yet been liquidated either by an offsetting derivative transaction or by delivery.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int OpenInterest { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public QuoteOptionsData()
        {
        }
        internal QuoteOptionsData(string symb, QuoteOptionType typ, double strike, double last, double cng, double b, double a, int vol, int interest)
        {
            this.Symbol = symb;
            this.Type = typ;
            this.StrikePrice = strike;
            this.LastPrice = last;
            this.Change = cng;
            this.Bid = b;
            this.Ask = a;
            this.Volume = vol;
            this.OpenInterest = interest;
        }

        public override string ToString()
        {
            return this.Symbol;
        }

    }
}
