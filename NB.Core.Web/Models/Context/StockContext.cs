using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class StockContext
    {
        // FA ValuationDataPoint Yahoo
        // PA analyst ratings up/downgrade http://www.analystratings.net/  find Ass and Ax analyst
        // BA Business momentum analysis yahoo Analyst Estimate, Zack Cash Flow growth
        // TA input: dataPoints[], process: scan pattern, strength, statistics (space, time), 
        //output TradingStatisticsAggregate (Type: Range, Uptrend, Downtrend, Consecutive Days Up, Consecutive Days Down, Up points, Down Points,Weekday up/down,  )
        // TradingStrategy {TradingStatisticsAggregate,Enter: buy price, qty(kelly), 
        //Exit: sell price , Stop: stop price, 
        //Time Window: holding period, Winning: Rate }
        // Trefis is the filter
        // Neural Network for earning play pattern
        // Calendar cycle
        // Stock Correlation
        // Map (GS opinion manual set)
        string[] _tickers;
        /*
         Yahoo Sector, Industry ValuationDataPoint
         MorningStar ValudationDataPoint Sector, Current Forward
         Nasdaq Earning Forecast SnapShot
         * 
        var fairValue = FairValueEngine.DiscountedCurrentValue(eps, 3, growthRate / 100.0, inflation, fixincomeReturnRate);

         */
        ConcurrentQueue<string> _exceptions = new ConcurrentQueue<string>();
        static Dictionary<string, NasdaqEarningForecastAggregate> _nasdaq = new Dictionary<string, NasdaqEarningForecastAggregate>(50);
        static Dictionary<string, MoringStartValuationAggregate> _morningStar = new Dictionary<string, MoringStartValuationAggregate>(50);
        static Dictionary<string, ValuationDataPointAggregate> _yahoo = new Dictionary<string, ValuationDataPointAggregate>(50);

        public StockContext(params string[] tickers)
        {
            _tickers = tickers;
            Parallel.ForEach(tickers, ticker =>
            {
                try
                {
                    this.PopulateYahooValuationDataPoint(ticker).Wait();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    _exceptions.Enqueue(ticker + " : yahoo 404");
                }

                try
                {
                    this.PopulateNasdaqValuationDataPoint(ticker).Wait();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    _exceptions.Enqueue(ticker + " : nasdaq 404");
                }

                try
                {
                    this.PopulateMorningStarValuationDataPoint(ticker).Wait();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    _exceptions.Enqueue(ticker + " : morningstar 404");
                }
            });                      
        }

        ValuationDataPointAggregate _yahooValDataPoint;

        NasdaqEarningForecastAggregate _nasdaqEarningForecast;

        MoringStartValuationAggregate _morningStarValuationMetric;

        public MorningStarValuation CurrentMorningStarValuationMetric
        {
            get { return _morningStarValuationMetric.CurrentValuation; }
        }

        public MorningStarValuation ForwardMorningStarValuationMetric
        {
            get { return _morningStarValuationMetric.ForwardValuation; }
        }

        public ValuationDataPoint SectorByTicker(string ticker)        
        {
            if (!_yahoo.ContainsKey(ticker))
            {
               PopulateYahooValuationDataPoint(ticker).Wait();
            }
            return _yahoo[ticker].Sector;
        }

        public ValuationDataPoint IndustryByTicker(string ticker)
        {
            if (!_yahoo.ContainsKey(ticker))
            {
                PopulateYahooValuationDataPoint(ticker).Wait();
            }
            return _yahoo[ticker].Industry;
        }

        public ValuationDataPoint EquityByTicker(string ticker)
        {
            if (!_yahoo.ContainsKey(ticker))
            {
                PopulateYahooValuationDataPoint(ticker).Wait();
            }
            return _yahoo[ticker].Self;
        }

        public NasdaqEarningForecastAggregate EarningForecastByTicker (string ticker)
        {
            if (!_nasdaq.ContainsKey(ticker))
            {
                PopulateNasdaqValuationDataPoint(ticker).Wait();
            }
            return _nasdaq[ticker];
        }

        public DateTime Date { get; set; }

        public async Task PopulateYahooValuationDataPoint(string ticker)
        {
            var setting = new YahooValuationSetting();
            var downloader = new YahooValuationDownloader(setting);
            _yahooValDataPoint = await downloader.DownloadObjectTaskAsync(setting.GetUrl(ticker)).ConfigureAwait(false);
            if (!_yahoo.ContainsKey(ticker))
                _yahoo.Add(ticker, _yahooValDataPoint);
        }

        public async Task PopulateNasdaqValuationDataPoint (string ticker)
        {
            var setting = new NasdaqEarningForecastSetting(ticker);
            var downloader = new NasdaqEarningForecastDownloader(setting);
            _nasdaqEarningForecast = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            if (!_nasdaq.ContainsKey(ticker))
                 _nasdaq.Add(ticker, _nasdaqEarningForecast);        
        }

        public async Task PopulateMorningStarValuationDataPoint(string ticker)
        {
            var setting = new MorningStarValuationSetting(ticker);
            var downloader = new MorningStartValuationDownloader(setting);
            var current = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);

            _morningStarValuationMetric = new MoringStartValuationAggregate(ticker);
            _morningStarValuationMetric.CurrentValuation = current;
            setting.IsForwardValuation = true;

            var forward = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            _morningStarValuationMetric.ForwardValuation = forward;

            _morningStar.Add(ticker, _morningStarValuationMetric);    
        }

    }
}
