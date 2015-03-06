using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class EarningHistoryResult
    {
        private EarningHistoryData[] mItems = null;

        public string  Ticker { get; set; }
        public EarningHistoryData[] Items
        {
            get { return mItems; }
        }

        internal EarningHistoryResult(EarningHistoryData[] items, string ticker="")
        {
            mItems = items;
            Ticker = ticker;
        }

        public double QuartylyEarningGrowth
        {
            get
            {
                var totalGrowth = mItems.Last().EarningActual - mItems.First().EarningActual;
                var howManyYears = mItems.Length;
                return Math.Pow(totalGrowth , 1.0/howManyYears);
            }
        }
    }
}
