﻿using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models.Mapping
{
    public class PriceDataYahooMapping : CsvClassMap<PriceDataPoint>
    {
        public PriceDataYahooMapping()
        {
            Map(m => m.Timestamp).Name("Date").Index(0);
            Map(m => m.Open).Name("Open").Index(1);//.TypeConverter<CustomTypeTypeConverter>();
            Map(m => m.High).Name("High").Index(2);
            Map(m => m.Low).Name("Low").Index(3);
            Map(m => m.Close).Name("Close").Index(4);
            Map(m => m.Volume).Name("Volume").Index(5);
            Map(m => m.Adjust).Name("Adj Close").Index(6);
        }
    }
}
