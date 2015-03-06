using NB.Core.Web.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class MorningStartValuation
    {
        public MorningStartValuation(string ticker)
        {
            Ticker = ticker;
        }
        public string Ticker { get; set; }

        //*[@id="currentValuationTable"]/tbody/tr[2]/td[1]
        [XPath("/tr[2]/td[1]", Name = "Current", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float CurrentPE { get; set; }

        [XPath("/tr[2]/td[3]", Name = "Current", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float CurrentSP500PE { get; set; }

        [XPath("/tr[2]/td[4]", Name = "Current", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float FiveYearsAvgPE { get; set; }

        [XPath("/tr[4]/td[1]", Name = "Current", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float CurrentPB { get; set; }

        [XPath("/tr[4]/td[3]", Name = "Current", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float CurrentSP500PB { get; set; }

        [XPath("/tr[4]/td[4]", Name = "Current", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float FiveYearsAvgPB { get; set; }

        [XPath("/tr[6]/td[1]", Name = "Current", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float CurrentPS { get; set; }

        [XPath("/tr[6]/td[3]", Name = "Current", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float CurrentSP500PS { get; set; }

        [XPath("/tr[6]/td[4]", Name = "Current", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float FiveYearsAvgPS { get; set; }

        ////*[@id="forwardValuationTable"]/tbody/tr[2]/td[2]
        [XPath("/tr[2]/td[2]", Name = "Forward", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float ForwardPE { get; set; }

        [XPath("/tr[2]/td[4]", Name = "Forward", Source = "http://financials.morningstar.com/valuation/price-ratio.html?t={0}&region=usa&culture=en-US")]
        public float ForwardSP500PE { get; set; }
    }


}
