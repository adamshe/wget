using CsvHelper.Configuration;
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models.Mapping
{
    public class PriceDataGoogleMapping : CsvClassMap<PriceDataPoint>
    {
        public static int Interval { get; set; }

        public static int TimeZoneOffset { get; set; }

        public static long TimeStampBase { get; set; }
        public PriceDataGoogleMapping()
        {
            //DateTime now = MyHelper.DateTimeFromUnixTimestamp(1425047460, 60, -300);
            //Map(m => m.Timestamp).Name("Timestamp").Index(0);   
            Map(m => m.Timestamp).Index(0).ConvertUsing(row =>
            {
                var unixstamp = row.GetField(0);
                var timeStamp = long.Parse(MyHelper.ExtractPattern(unixstamp, @"[a-zA-Z]?(\d+)$$"));
                if (timeStamp > TimeStampBase)
                    TimeStampBase = timeStamp;
                else
                    timeStamp = TimeStampBase + Interval * timeStamp;

                var datetime = MyHelper.DateTimeFromUnixTimestamp(timeStamp, TimeZoneOffset);
                return datetime;
            });
            Map(m => m.Open).Index(1);//.TypeConverter<CustomTypeTypeConverter>();
            Map(m => m.High).Index(2);
            Map(m => m.Low).Index(3);
            Map(m => m.Close).Index(4);
            Map(m => m.Volume).Index(5);
                   
        }
    }
}
