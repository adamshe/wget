using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wget
{
    class Program
    {
        static void Main(string[] args)
        {
            TestConfiguration();
        }

        static void Download(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();
            var list = args.Length > 1 ? args[1] : ConfigurationManager.AppSettings["tickers"];
            if (string.Compare(args[0], "e", true) == 0)
            {
                DownloadEarning(list).Wait();
            }
            else if (string.Compare(args[0], "f", true) == 0)
            {
                DownloadHistoryCsv(list).Wait();
            }
            watch.Stop();
            Console.WriteLine(string.Format("{0} seconds", watch.ElapsedMilliseconds / 1000.0));
        }

        static void TestConfiguration()
        {
            //ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            //fileMap.ExeConfigFilename = @"SpyData.config";  // relative path names possible
            //// Open another config file
            //Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            //var data = new DataSectionManager();
            //data.Name = "SPY";//.Date = new DateTime(2015, 3, 15);
            //data.MetricsType = "S&P PE";
            
            //// read/write from it as usual
            //ConfigurationSection mySection = config.GetSection("SpyDataSource");            
            //config.SectionGroups.Clear(); // make changes to it
            
            //config.Save(ConfigurationSaveMode.Full, );  // Save changes

            DatapointCollection settingsCollection = new DatapointCollection();
            settingsCollection[0] = new DataElement { Date = new DateTime(2015, 3, 5), Value = 19.85f };
            settingsCollection[1] = new DataElement { Date = new DateTime(2014, 1, 1), Value = 18.3f };
            settingsCollection[2] = new DataElement { Date = new DateTime(2013, 1, 1), Value = 16f };
            var config = new DataSectionManager("DataSource");
            config.CreateDefaultConfigurationSection("DataSource", settingsCollection);

        }
        static async Task DownloadHistoryCsv (string tickers)
        {
            var setting = new YahooHistoryCsvSetting("SPY");
            var downloader = new YahooHistoryCsvDownloader(setting);
            var earningDates = await downloader.BatchDownloadFilesTaskAsync(
                setting.GetUrls(tickers)).ConfigureAwait(false);
        }

        static async Task DownloadEarning(string tickers)
        {
            var setting = new FinvizEarningCalendarSetting("SPY");
            var downloader = new FinvizEarningCalendarDownloader(setting);
            var earningDates = await downloader.BatchDownloadObjectsTaskAsync(
                setting.GetUrls(tickers)).ConfigureAwait(false);

            var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\earningCalendar.txt";
            using (StreamWriter file = File.CreateText(filePath))
            {
                foreach (var earning in earningDates)
                    file.WriteLine(earning.Ticker.PadRight(5) + " " + earning.ToString());
            }
//                @"AAPL,YHOO,MSFT,GOOGL,CYBR,BA,ADBE,HDP,NEWR,WYNN,LVS,tsla,nflx,pcln,amzn,
//            FB,LNKD,TWTR,JD,JMEI,TKMR,CELG,BIIB,ICPT,PCYC,INCY,DATA,NOW,GILD,SPLK,TSO,
//            LNG,EOG,APC,GPRO,MSFT,CSCO,BRCM,INTC,NUAN,RCL,CYBR,MCO,DFS,AXP,MA,V,GS,BAC,
//            C,JUNO,KITE,BLUE,GMCR")
        }

    }
}
