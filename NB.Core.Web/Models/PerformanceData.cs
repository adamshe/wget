using NB.Core.Web.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class PerformanceDataAggregate
    {
        public PerformanceDataAggregate(string ticker)
        {
            Ticker = ticker;
        }
        public string Ticker { get; set; }

        public PerformanceData StockPerformance { get; set; }

        public PerformanceData Industryformance { get; set; }

        public PerformanceData SP500formance { get; set; }
    }

    public class PerformanceData
    {       
        public string Ticker { get; set; }

        [XPath("/tr[1]/td[1]", Name = "Stock")]
        [XPath("/tr[2]/td[1]", Name = "Industry")]
        [XPath("/tr[3]/td[1]", Name = "SP500")]
        public float Today { get; set; }

        [XPath("/tr[1]/td[2]", Name = "Stock")]
        [XPath("/tr[2]/td[2]", Name = "Industry")]
        [XPath("/tr[3]/td[2]", Name = "SP500")]
        public float OneWeek { get; set; }

        [XPath("/tr[1]/td[3]", Name = "Stock")]
        [XPath("/tr[2]/td[3]", Name = "Industry")]
        [XPath("/tr[3]/td[3]", Name = "SP500")]
        public float OneMonth { get; set; }

        [XPath("/tr[1]/td[4]", Name = "Stock")]
        [XPath("/tr[2]/td[4]", Name = "Industry")]
        [XPath("/tr[3]/td[4]", Name = "SP500")]
        public float ThreeMonth { get; set; }

        [XPath("/tr[1]/td[5]", Name = "Stock")]
        [XPath("/tr[2]/td[5]", Name = "Industry")]
        [XPath("/tr[3]/td[5]", Name = "SP500")]
        public float YTD { get; set; }

        [XPath("/tr[1]/td[6]", Name = "Stock")]
        [XPath("/tr[2]/td[6]", Name = "Industry")]
        [XPath("/tr[3]/td[6]", Name = "SP500")]
        public float OneYear { get; set; }

        [XPath("/tr[1]/td[7]", Name = "Stock")]
        [XPath("/tr[2]/td[7]", Name = "Industry")]
        [XPath("/tr[3]/td[7]", Name = "SP500")]
        public float ThreeYear { get; set; }

        [XPath("/tr[1]/td[8]", Name = "Stock")]
        [XPath("/tr[2]/td[8]", Name = "Industry")]
        [XPath("/tr[3]/td[8]", Name = "SP500")]
        public float FiveYear { get; set; }

        [XPath("/tr[1]/td[9]", Name = "Stock")]
        [XPath("/tr[2]/td[9]", Name = "Industry")]
        [XPath("/tr[3]/td[9]", Name = "SP500")]
        public float TenYear { get; set; }

        [XPath("/tr[1]/td[10]", Name = "Stock")]
        [XPath("/tr[2]/td[10]", Name = "Industry")]
        [XPath("/tr[3]/td[10]", Name = "SP500")]
        public float FifteenYear { get; set; }
        
    }
}
