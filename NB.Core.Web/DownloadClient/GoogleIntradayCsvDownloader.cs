using CsvHelper;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Models.Mapping;
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadClient
{
    public class GoogleIntradayCsvDownloader : BaseDownloader<IEnumerable<PriceDataPoint>>
    {
        private const string columnSeparator = ";";
        public GoogleIntradayCsvDownloader(BaseSetting setting)
            : base(setting)
        {
            
        }


        protected override IEnumerable<PriceDataPoint> ConvertResult(StreamReader reader, string ticker = "")
        {
            var list = new List<PriceDataPoint>(200);
            string headerLine;
            headerLine = reader.ReadLine();
            headerLine = reader.ReadLine();
            headerLine = reader.ReadLine();
            headerLine = reader.ReadLine();
            ((GoogleIntradayCsvSetting)Setting).TimezoneOffset = int.Parse(MyHelper.ExtractPattern(headerLine, @".*=(\d*)"));
            headerLine = reader.ReadLine();
            headerLine = reader.ReadLine();
            headerLine = reader.ReadLine();
            
            /*
                * EXCHANGE%3DNYSE
                MARKET_OPEN_MINUTE=570
                MARKET_CLOSE_MINUTE=960
                INTERVAL=60
                COLUMNS=DATE,CLOSE,HIGH,LOW,OPEN,VOLUME
                DATA=
                TIMEZONE_OFFSET=-300
                */
               
            using (var csvReader = new CsvReader(reader))
            {
                csvReader.Configuration.HasHeaderRecord = false;
                csvReader.Configuration.RegisterClassMap<PriceDataGoogleMapping>();
                while (csvReader.Read())
                {
                    var data = csvReader.GetRecord<PriceDataPoint>();
                    list.Add(data);
                }
            }
            
            return list.ToArray<PriceDataPoint>();
        }

        protected override IEnumerable<PriceDataPoint> ConvertResult(string contentStr, string ticker = "")
        {
            //https://github.com/JoshClose/CsvHelper/blob/master/src/CsvHelper.Example/Program.cs
            var list = new List<PriceDataPoint>(200);
            using (var reader = MyHelper.GetStreamReader(contentStr))
            {
                /*
                 * EXCHANGE%3DNYSE
                    MARKET_OPEN_MINUTE=570
                    MARKET_CLOSE_MINUTE=960
                    INTERVAL=60
                    COLUMNS=DATE,CLOSE,HIGH,LOW,OPEN,VOLUME
                    DATA=
                    TIMEZONE_OFFSET=-300
                 */
                string headerLine;
                headerLine = reader.ReadLine();
                headerLine = reader.ReadLine();
                headerLine = reader.ReadLine();
                headerLine = reader.ReadLine();
                ((GoogleIntradayCsvSetting)Setting).TimezoneOffset = int.Parse(MyHelper.ExtractPattern(headerLine, @".*=(\d*)"));
                headerLine = reader.ReadLine();
                headerLine = reader.ReadLine();
                headerLine = reader.ReadLine();
                using (var csvReader = new CsvReader(reader))
                {
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.RegisterClassMap<PriceDataGoogleMapping>();
                    while (csvReader.Read())
                    {
                        var data = csvReader.GetRecord<PriceDataPoint>();
                        list.Add(data);
                    }
                }
            }
            return list.ToArray<PriceDataPoint>();
        }
    }
}
