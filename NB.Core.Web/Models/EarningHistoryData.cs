using NB.Core.Web.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class EarningHistoryData
    {
        [XPath("/td[2]", Name = "Nasdaq", Source = "http://www.nasdaq.com/earnings/report/{0}")]////*[@id="showdata-div"]/div[3]/table/tbody/tr[2]/td[1]
        public DateTime ReportDate { get; set; }

        [XPath("/td[3]", Name = "Nasdaq", Source = "http://www.nasdaq.com/earnings/report/{0}")]
        public float EarningActual { get; set; }

        [XPath("/td[4]", Name = "Nasdaq", Source = "http://www.nasdaq.com/earnings/report/{0}")]
        public float EarningConsensusForecast { get; set; }

        public float SurprisePercentage { get { return EarningActual / EarningConsensusForecast - 1; } }
    }
}
