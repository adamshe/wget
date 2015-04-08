using NB.Core.Web.Models.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class AnalystRatingsDataAggregate
    {
        private AnalystRatingsData[] _actions;

        public AnalystRatingsDataAggregate(AnalystRatingsData[] actions)
        {
            _actions = actions;
        }

        public AnalystRatingsData[] AnalystRatings { get { return _actions; } }
    }

    public class AnalystRatingsData
    {
        public AnalystRatingsData()
        {

        }
        [XPath("/td[1]")] 
        public DateTime Date { get; set; }

        [XPath("/td[2]")] 
        public string Firm { get; set; }

        [XPath("/td[3]")] 
        public string Action { get; set; }

        [XPath("/td[4]")] 
        public string Rating { get; set; }

        [XPath("/td[5]")] 
        public PriceTarget PriceAction { get; set; }
    }

    [TypeConverter(typeof(PriceTargetConverter))]
    public class PriceTarget 
    {
        public float OriginalPrice { get; set; }

        public float TargetPrice { get; set; }
    }

    public class PriceTargetConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var priceTarget = new PriceTarget();

            var priceRange = value as string;
            
            if ( !string.IsNullOrEmpty(priceRange))
            {   
                string[] prices = priceRange.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                if (prices.Length == 2)
                {
                    priceTarget.TargetPrice = float.Parse(prices[1].Trim(), NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint);
                    priceTarget.OriginalPrice = float.Parse(prices[0].Trim(), NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint);
                }
                else if (prices.Length == 1)
                {
                    priceTarget.TargetPrice = float.Parse(prices[0].Trim(), NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint);
                }
            }
            return priceTarget;
        }
    }
}
