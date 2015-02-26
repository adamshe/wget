using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class QuotesBaseData
    {

        private string mID = string.Empty;

        /// <summary>
        /// The ID of the QuoteBaseData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual string ID
        {
            get { return mID; }
        }
        /// <summary>
        /// Sets a new ID value. Implementation from ISettableID.
        /// </summary>
        /// <param name="id">A valid Yahoo! ID</param>
        /// <remarks></remarks>
        public virtual void SetID(string id)
        {
            mID = id;
        }
        public virtual string Name { get; set; }
        /// <summary>
        /// The price value of the QuoteBaseData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual double LastTradePriceOnly { get; set; }
        /// <summary>
        /// The change of the price in relation to close value of the previous day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual double Change { get; set; }
        /// <summary>
        /// The trade volume of the day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual long Volume { get; set; }
        /// <summary>
        /// The calculated close price of the last trading day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>[LastTradePriceOnly] - [Change]</remarks>
        public virtual double PreviewClose
        {
            get { return this.LastTradePriceOnly - this.Change; }
        }
        /// <summary>
        /// The calculated price change in percent
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>[Change] / [PreviewClose]</remarks>
        public virtual double ChangeInPercent
        {
            get
            {
                if (this.PreviewClose != 0)
                {
                    return (this.Change / this.PreviewClose) * 100;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// The date value of the data
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual System.DateTime LastTradeDate { get; set; }
        /// <summary>
        /// The time value of the data
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual System.DateTime LastTradeTime { get; set; }

        public QuotesBaseData()
        {
            mID = string.Empty;
            this.Name = string.Empty;
            this.LastTradePriceOnly = 0;
            this.Change = 0;
            this.Volume = 0;
            this.LastTradeDate = new DateTime();
            this.LastTradeTime = new DateTime();
        }

    }
}
