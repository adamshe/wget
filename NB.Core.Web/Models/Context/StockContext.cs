using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class StockContext
    {
        public string Ticker { get; set; }

        public ValuationDataPoint Sector { get; set; }

        public ValuationDataPoint Industry { get; set; }

        public DateTime Date { get; set; }
    }
}
