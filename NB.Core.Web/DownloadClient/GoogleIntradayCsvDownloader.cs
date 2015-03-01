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
    public class GoogleIntradayCsvDownloader : BaseDownloader<IEnumerable<PriceData>>
    {
        private const string columnSeparator = ";";
        public GoogleIntradayCsvDownloader(BaseSetting setting)
            : base(setting)
        {
            
        }

        protected override IEnumerable<PriceData> ConvertResult(StreamReader sr, string ticker = "")
        {
            var list = new List<PriceData>(200);
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            
            /*
                * EXCHANGE%3DNYSE
                MARKET_OPEN_MINUTE=570
                MARKET_CLOSE_MINUTE=960
                INTERVAL=60
                COLUMNS=DATE,CLOSE,HIGH,LOW,OPEN,VOLUME
                DATA=
                TIMEZONE_OFFSET=-300
                */
               
            using (var csvReader = new CsvReader(sr))
            {
                csvReader.Configuration.RegisterClassMap<PriceDataYahooMapping>();
                while (csvReader.Read())
                {
                    var data = csvReader.GetRecord<PriceData>();
                    list.Add(data);
                }
            }
            
            return list.ToArray<PriceData>();
        }

        protected override IEnumerable<PriceData> ConvertResult(string contentStr, string ticker = "")
        {
            //https://github.com/JoshClose/CsvHelper/blob/master/src/CsvHelper.Example/Program.cs
            var list = new List<PriceData>(200);
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
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                using (var csvReader = new CsvReader(reader))
                {
                    csvReader.Configuration.RegisterClassMap<PriceDataYahooMapping>();
                    while (csvReader.Read())
                    {
                        var data = csvReader.GetRecord<PriceData>();
                        list.Add(data);
                    }
                }
            }
            return list.ToArray<PriceData>();
        }
    }
}
