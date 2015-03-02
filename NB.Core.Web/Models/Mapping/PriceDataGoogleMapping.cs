using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models.Mapping
{
    public class PriceDataGoogleMapping : CsvClassMap<PriceData>
    {
        public PriceDataGoogleMapping()
        {
            //Map(m => m.Timestamp).Name("Timestamp").Index(0);   
            Map(m => m.Timestamp).Index(0);   
            Map(m => m.Open).Index(1);//.TypeConverter<CustomTypeTypeConverter>();
            Map(m => m.High).Index(2);
            Map(m => m.Low).Index(3);
            Map(m => m.Close).Index(4);
            Map(m => m.Volume).Index(5);
                   
        }
    }
}
