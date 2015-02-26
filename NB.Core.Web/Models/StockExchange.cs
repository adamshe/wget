using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class StockExchange
    {

        private string mID = string.Empty;
        private string mName = string.Empty;
        private string mSuffix = string.Empty;

        private CountryInfo mCountry = null;

        private TradingTimeInfo mTradingTime = null;
        /// <summary>
        /// The ID of the exchange
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>If the ID is in WorldMarket.DefaultStockExchanges, properties will be setted automatically</remarks>
        public string ID
        {
            get { return mID; }
        }

        /// <summary>
        /// The ending string for stock IDs
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>If the suffix is in DefaultStockExchanges, properties will get automatically</remarks>
        public string Suffix
        {
            get { return mSuffix; }
        }

        /// <summary>
        /// The name of the exchange
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get { return mName; }
        }


        public CountryInfo Country
        {
            get { return mCountry; }
        }


        public TradingTimeInfo TradingTime
        {
            get { return mTradingTime; }
        }

        private readonly List<string> mTags = new List<string>();
        internal List<string> Tags
        {
            get { return mTags; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="suffix"></param>
        /// <remarks></remarks>
        public StockExchange(string id, string suffix, string name, CountryInfo country, TradingTimeInfo tradeTime)
        {
            if (id != string.Empty)
            {
                mID = id;
            }
            else
            {
                throw new ArgumentNullException("id", "The ID is empty.");
            }

            mSuffix = suffix;
            mName = name;
            if (country != null)
            {
                mCountry = country;
            }
            else
            {
                throw new ArgumentNullException("country", "The country is null.");
            }

            if (tradeTime != null)
            {
                mTradingTime = tradeTime;
            }
            else
            {
                throw new ArgumentNullException("tradeTime", "The trade time is null.");
            }
        }
        internal StockExchange(StockExchange se)
        {
            if (se == null)
            {
                throw new ArgumentNullException("se", "Original StockExchange is null.");
            }
            else
            {
                if (se != null)
                {
                    mID = se.ID;
                    mCountry = se.Country;
                    mSuffix = se.Suffix;
                    mName = se.Name;
                    TradingTimeInfo tt = se.TradingTime;
                    mTradingTime = new TradingTimeInfo(tt.DelayMinutes, tt.TradingDays, tt.Holidays, tt.LocalOpeningTime, tt.TradingSpan, tt.UtcOffsetStandardTime, tt.DaylightSavingTimes);
                }
            }
        }

        /// <summary>
        /// Returns the name of the stock exchange
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return mName;
        }

    }
}
