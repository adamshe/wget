
using System;
using System.Diagnostics;
namespace wget
{
    //http://dotnetregexevaluator.andreasandersen.dk/ regex
    class Program
    {
        static YahooDownloadParam param;
        static void Main(string[] args)
        {
            param = new YahooDownloadParam(args);

            var watch =  Stopwatch.StartNew();
            watch.Start();
            //YahooCsvDownloader.StartDownload(param);
            //watch.Stop();
            //Console.WriteLine(watch.ElapsedMilliseconds / 1000.0);
            //Console.WriteLine("Download Complete!");

            watch.Restart();
            YahooCsvDownloader.BatchDownloadFiles(param.GetUrls()).Wait();
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds / 1000.0);
            Console.WriteLine("Download Complete!");
            
        }

    }
}
