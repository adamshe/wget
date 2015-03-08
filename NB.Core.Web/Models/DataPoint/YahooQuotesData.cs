using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class YahooQuotesAggregate : QuotesBaseAggregate
    {
        private YahooQuotesSettings mSettings = null;
        public YahooQuotesSettings Settings { get { return mSettings; } }

        public new YahooQuotesData[] Items
        {
            get
            {
                return base.Items.Cast<YahooQuotesData>().ToArray();
            }
        }

        public void SortBy(QuoteProperty property)
        {
            try
            {
                base.Items = base.Items.OrderBy(item => ((YahooQuotesData)item)[property] ?? double.MinValue).ToArray();
            }
            catch (Exception ex)
            {

            }
        }

        internal YahooQuotesAggregate(YahooQuotesData[] items, YahooQuotesSettings settings)
            : base(items)
        {
            mSettings = settings;
        }
    }

    public class YahooQuotesData : QuotesBaseData, ICloneable
    {

        private object[] mValues = new object[88];

        public object Values(QuoteProperty prp) { return this[prp]; }
        /// <summary>
        /// Gets or sets the value of a specfic property
        /// </summary>
        /// <param name="prp">Gets or sets the property you want to get or set</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public object this[QuoteProperty prp]
        {
            get { return mValues[(int)prp]; }
            set
            {
                if (value != null)
                {
                    double t = 0;
                    System.DateTime dt;
                    long l = 0;
                    switch (prp)
                    {
                        case QuoteProperty.Symbol:
                            base.SetID(value.ToString());
                            break;
                        case QuoteProperty.LastTradePriceOnly:
                            if (double.TryParse(value.ToString(), out t))
                                base.LastTradePriceOnly = t;
                            break;
                        case QuoteProperty.LastTradeDate:
                            if (System.DateTime.TryParse(value.ToString(), out dt))
                                base.LastTradeDate = dt;

                            break;
                        case QuoteProperty.LastTradeTime:
                            if (System.DateTime.TryParse(value.ToString(), out dt))
                                base.LastTradeTime = dt;
                            break;
                        case QuoteProperty.Change:
                            if (double.TryParse(value.ToString(), out t))
                                base.Change = t;
                            break;
                        case QuoteProperty.Volume:
                            if (long.TryParse(value.ToString(), out l))
                                base.Volume = l;
                            break;
                        case QuoteProperty.Name:
                            base.Name = value.ToString();
                            break;
                    }
                 mValues[(int)prp] = string.IsNullOrWhiteSpace(value.ToString())?null:value;
               }
            }
        }

        /// <summary>
        /// Gets or sets the name of the QuoteData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Represents the value of QuoteProperty.Name</remarks>
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; mValues[(int)QuoteProperty.Name] = value; }
        }
        /// <summary>
        /// Sets a new ID value. Implementation from ISettableID.
        /// </summary>
        /// <param name="id">A valid Yahoo! ID</param>
        /// <remarks></remarks>
        public override void SetID(string id)
        {
            base.SetID(id);
            mValues[(int)QuoteProperty.Symbol] = id;
        }
        /// <summary>
        /// Gets or sets the latest price value of the QuoteData.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Represents the value of QuoteProperty.LastTradePriceOnly</remarks>
        public override double LastTradePriceOnly
        {
            get { return base.LastTradePriceOnly; }
            set { base.LastTradePriceOnly = value; mValues[(int)QuoteProperty.LastTradePriceOnly] = value; }
        }
        /// <summary>
        /// Gets or sets the date value of the last trade.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override System.DateTime LastTradeDate
        {
            get { return base.LastTradeDate; }
            set
            { base.LastTradeDate = value; mValues[(int)QuoteProperty.LastTradeDate] = value; }
        }
        /// <summary>
        /// Gets or sets the time value of the last trade.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override DateTime LastTradeTime
        {
            get { return base.LastTradeTime; }
            set { base.LastTradeTime = value; mValues[(int)QuoteProperty.LastTradeTime] = value; }
        }
        /// <summary>
        /// Gets or sets the change in percent.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override double Change
        {
            get { return base.Change; }
            set { base.Change = value; mValues[(int)QuoteProperty.Change] = value; }
        }
        /// <summary>
        /// Gets or sets the trade volume of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override long Volume
        {
            get { return base.Volume; }
            set { base.Volume = value; mValues[(int)QuoteProperty.Volume] = value; }
        }
        /// <summary>
        /// Gets or sets the opening price value of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Open
        {
            get { if (mValues[(int)QuoteProperty.Open] != null) { return (double)mValues[(int)QuoteProperty.Open]; } else { return 0; } }
            set { mValues[(int)QuoteProperty.Open] = value; }
        }
        /// <summary>
        /// Gets or sets the highest value of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double DaysHigh
        {
            get { if (mValues[(int)QuoteProperty.DaysHigh] != null) { return (double)mValues[(int)QuoteProperty.DaysHigh]; } else { return 0; } }
            set { mValues[(int)QuoteProperty.DaysHigh] = value; }
        }
        /// <summary>
        /// Gets or sets the lowest price value of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double DaysLow
        {
            get { if (mValues[(int)QuoteProperty.DaysLow] != null) { return (double)mValues[(int)QuoteProperty.DaysLow]; } else { return 0; } }
            set { mValues[(int)QuoteProperty.DaysLow] = value; }
        }
        public string Currency
        {
            get { return this[QuoteProperty.Currency] != null ? this[QuoteProperty.Currency].ToString() : string.Empty; }
            set { this[QuoteProperty.Currency] = value; }
        }

        public override double PreviewClose
        {
            get
            {
                if (this[QuoteProperty.PreviousClose] != null && this[QuoteProperty.PreviousClose] is double)
                {
                    return Convert.ToDouble(this[QuoteProperty.PreviousClose]);
                }
                else
                {
                    return base.PreviewClose;
                }
            }
        }
        public override double ChangeInPercent
        {
            get
            {
                if (this[QuoteProperty.ChangeInPercent] != null && this[QuoteProperty.ChangeInPercent] is double)
                {
                    return Convert.ToDouble(this[QuoteProperty.ChangeInPercent]);
                }
                else
                {
                    return base.ChangeInPercent;
                }
            }
        }

        public YahooQuotesData() { }

        public YahooQuotesData(string id) { this.SetID(id); }

        public virtual object Clone()
        {
            YahooQuotesData cln = new YahooQuotesData();
            foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
            {
                if (this[qp] is object[])
                {
                    object[] obj = (object[])this[qp];
                    object[] newObj = new object[obj.Length];
                    if (obj.Length > 0)
                    {
                        for (int i = 0; i <= obj.Length - 1; i++)
                        {
                            newObj[i] = obj[i];
                        }
                    }
                    cln[qp] = newObj;
                }
                else
                {
                    cln[qp] = this[qp];
                }
            }
            return cln;
        }       
    }

}
