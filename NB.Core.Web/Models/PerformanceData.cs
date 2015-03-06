using NB.Core.Web.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class PerformanceData
    {       
        public string Ticker { get; set; }

        [XPath("/td[1]")]
        public float Today { get; set; }

        [XPath("/td[2]")]
        public float OneWeek { get; set; }

        [XPath("/td[3]")]
        public float OneMonth { get; set; }

        [XPath("/td[4]")]
        public float ThreeMonth { get; set; }

        [XPath("/td[5]")]
        public float YTD { get; set; }

        [XPath("/td[6]")]
        public float OneYear { get; set; }

        [XPath("/td[7]")]
        public float ThreeYear { get; set; }

        [XPath("/td[7]")]
        public float FiveYear { get; set; }

        [XPath("/td[8]")]
        public float TenYear { get; set; }

        [XPath("/td[9]")]
        public float FifteenYear { get; set; }
        
    }
}
