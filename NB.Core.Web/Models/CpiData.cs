using NB.Core.Web.Models.Metadata;
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class CpiDataAggregate
    {
        public CpiDataAggregate(CpiData[] items)
        {
            Items = items;
        }

        public string Where { get { return "US"; } }

        public double CurentCpi 
        {
            get
            {
                var cpi = Items.Where(data => data.Year == DateTime.Now.Year).FirstOrDefault().GetLatestData();
                return cpi;
            }
        }

        public CpiData[] Items { get; set; }
    }

    /// <summary>
    /// bps in unit
    /// </summary>
    public class CpiData
    {
        [XPath("/td[1]")]
        public int Year { get; set; }

        [XPath("/td[2]")]
        public double Jan 
        { 
            get {                 
                return this[1]; 
            }
            set { this[1] = value; } 
        }

        [XPath("/td[3]")]
        public double Feb
        {
            get { return this[2]; }
            set { this[2] = value; }
        }

        [XPath("/td[4]")]
        public double Mar
        {
            get { return this[3]; }
            set { this[3] = value; }
        }

        [XPath("/td[5]")]
        public double Apr
        {
            get { return this[4]; }
            set { this[4] = value; }
        }

        [XPath("/td[6]")]
        public double May
        {
            get { return this[5]; }
            set { this[5] = value; }
        }

        [XPath("/td[7]")]
        public double Jun
        {
            get { return this[6]; }
            set { this[6] = value; }
        }

        [XPath("/td[8]")]
        public double Jul
        {
            get { return this[7]; }
            set { this[7] = value; }
        }

        [XPath("/td[9]")]
        public double Aug
        {
            get { return this[8]; }
            set { this[8] = value; }
        }

        [XPath("/td[10]")]
        public double Sep
        {
            get { return this[9]; }
            set { this[9] = value; }
        }

        [XPath("/td[11]")]
        public double Oct
        {
            get { return this[10]; }
            set { this[10] = value; }
        }

        [XPath("/td[12]")]
        public double Nov
        {
            get { return this[11]; }
            set { this[11] = value; }
        }

        [XPath("/td[13]")]
        public double Dec
        {
            get { return this[12]; }
            set { this[12] = value; }
        }

        [XPath("/td[14]")]
        public double Avg
        {
            get { return this[0]; }
            set { this[0] = value; }
        }

        double[] _cpi = new double[13];

        public double this[int index] 
        {
            get { return _cpi[index]; }
            set { _cpi[index] = value; }
        }

        public double this[string month]
        {
            get 
            {
                int index = DateTime.ParseExact(month, "MMM", MyHelper.DefaultCulture).Month;
                return _cpi[index]; 
            }
            set {
                int index = DateTime.ParseExact(month, "MMM", MyHelper.DefaultCulture).Month;
                _cpi[index] = value;             
            }
        }

        public double GetLatestData ()
        {
            for (int index = 12; index > 0; index--)
            {
                if (!double.IsNaN(_cpi[index]) && _cpi[index]!=0.0)
                    return _cpi[index];
            }
            return double.NaN;
        }
    }
}
