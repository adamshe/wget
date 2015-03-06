using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class PriceDataPoint
    {
        public string Timestamp { get; set; }

        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double  Close { get; set; }

        public long Volume { get; set; }

        public double Adjust { get; set; } 
    }
}
