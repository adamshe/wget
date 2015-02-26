using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class QuotesBaseResult
    {
        private QuotesBaseData[] mItems = null;
        public QuotesBaseData[] Items { get { return mItems; } protected set { mItems = value; } }

        public QuotesBaseResult(QuotesBaseData[] items)
        {
            mItems = items;
        }
    }
}
