using NB.Core.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class CountryInfo
    {
        private Country mID;
        private string mName = string.Empty;
        private CurrencyInfo mCurrency = null;
        private DaylightSavingTime[] mDaylightSavingTimes = new DaylightSavingTime[-1 + 1];

        private List<YIndexID> mIndices = new List<YIndexID>();
        /// <summary>
        /// The country ID.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Country ID
        {
            get { return mID; }
        }
        /// <summary>
        /// The currency of this country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo Currency
        {
            get { return mCurrency; }
        }
        /// <summary>
        /// The name of the country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get { return mName; }
        }
        /// <summary>
        /// The list of Daylight Saving Times of the country for each year
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DaylightSavingTime[] DaylightSavingTimes
        {
            get { return mDaylightSavingTimes; }
        }
        /// <summary>
        ///The indices of the country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<Support.YIndexID> Indices
        {
            get { return mIndices; }
        }

        public CountryInfo(Country id, string name, CurrencyInfo cur)
        {
            mID = id;
            mName = name;
            mCurrency = cur;
        }
        public CountryInfo(Country id, string name, CurrencyInfo cur, DaylightSavingTime[] dstArray)
            : this(id, name, cur)
        {
            if (dstArray != null)
                mDaylightSavingTimes = dstArray;
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
