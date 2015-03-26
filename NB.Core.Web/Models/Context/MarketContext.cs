using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class MarketContext
    {
        public float FairValue { get; set; }

        public float TenYearBond { get; set; }

        public float SpyTenYearReturn { get; set; }

        public Dictionary<string, SectorContext> IndustryDictionary { get; set; }
    }
}
