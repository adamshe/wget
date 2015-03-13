using NB.Core.Web.Models.DataPoint;
using NB.Core.Web.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class Institution
    {
        [XPath("/td[1]/a")]
        public string Name { get; set; }

        [XPath("/td[1]/a", Source="href", RegexExpression=@".*-\d+$")]
        public int InstitutionId { get; set; }

        public HoldingDataPoint [] Holding { get; set;}
    }
}
