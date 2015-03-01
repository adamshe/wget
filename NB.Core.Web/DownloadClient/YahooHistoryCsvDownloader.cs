﻿using CsvHelper;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Models.Mapping;
using NB.Core.Web.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace NB.Core.Web.DownloadClient
{
    public class YahooHistoryCsvDownloader : BaseDownloader<IEnumerable<PriceData>>
    {
        private const string columnSeparator = ";";
        public YahooHistoryCsvDownloader(BaseSetting setting)
            : base(setting)
        {
            
        }

        protected override IEnumerable<PriceData> ConvertResult(StreamReader sr, string ticker = "")
        {
            var list = new ConcurrentBag<PriceData>();
        //    var header = sr.ReadLine();
            using (var csvReader = new CsvReader(sr))
            {
                csvReader.Configuration.RegisterClassMap<PriceDataYahooMapping>();
                while (csvReader.Read())
                {
                    try
                    {
                        var data = csvReader.GetRecord<PriceData>();
                        list.Add(data);
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                    }
                }
                sr.Close();
            }
            return list.ToArray<PriceData>();
        }

        protected override IEnumerable<PriceData> ConvertResult(string contentStr, string ticker = "")
        {
            //https://github.com/JoshClose/CsvHelper/blob/master/src/CsvHelper.Example/Program.cs
            var list = new List<PriceData>();
            //var list = new ThreadLocal<List<PriceData>>();
            using (var reader = MyHelper.GetStreamReader(contentStr))
            using (var csvReader = new CsvReader (reader))
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
    }
}
