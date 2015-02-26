using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class TickerEarningDate : IComparable<TickerEarningDate>
    {
        public string Ticker { get; set; }
        public DateTime EarningDate { get; set; }
        public float Estimate { get; set; }
        public bool IsBeatEasy { get; set; }

        public override string ToString()
        {
            return EarningDate.ToString("MMM dd, yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public int CompareTo(TickerEarningDate other)
        {
            return this.EarningDate.CompareTo(other.EarningDate);
        }
    }
}
