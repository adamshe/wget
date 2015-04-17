using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Enums;
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class PriceDataPointAggregate
    {
        PriceDataPoint[] _dataPoints;
        public PriceDataPointAggregate(PriceDataPoint[] dataPoints)
        {            
           _dataPoints = dataPoints.ToArray();
        }

        public IEnumerator<PriceDataPoint> Forward
        {
            get
            {
                for (int i = 0 ; i < _dataPoints.Length ; i++)
                    yield return _dataPoints[i];
            }
        }

        public IEnumerator<PriceDataPoint> Backward
        {
            get
            {
                for (int i = _dataPoints.Length; i > 0; i--)
                    yield return _dataPoints[i];
            }
        }

    }

    public class PriceDataPoint : IEquatable<PriceDataPoint>
    {
        //public static string TimestampBase { get; set; }

        //public static int Interval { get; set; }

        //public static int  Offset { get; set; }

        //public string Timestamp { get; set; }

        public DateTime Timestamp { get; set; }

        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double  Close { get; set; }

        public long Volume { get; set; }

        public double Adjust { get; set; }

        #region Compute Value

        public double PriceOverVolume 
        {
            get 
            {
                double median;
                if (BullishIndex >= 0.5)
                    median = (Open + High + Close) / 3.0;
                else if (BullishIndex <= -0.5)
                    median = (Open + Low + Close) / 3.0;
                else
                    median = (Open + Low + High + Close) / 4.0;

                return median / Volume;
            } 
        }

        public double OpenCloseMedianPrice { get { return (Open + Close) / 2.0; } }

        public double HighLowMedianPrice { get { return (High + Low) / 2.0; } }

        public bool IsBullish { get { return OpenCloseMedianPrice - HighLowMedianPrice >= 0 && Close - Open > 0; } }

        public double BullishIndex { get { return (Close - Open) / (High - Low); } }

        #endregion

        #region Equality

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return this.Equals(obj as PriceDataPoint);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Timestamp.GetHashCode();
        }

        public bool Equals(PriceDataPoint other)
        {
            if ((object)other == null)
                return false;

            return Timestamp.Equals(other.Timestamp);
        }

        public static bool operator == (PriceDataPoint @this, PriceDataPoint other)
        {
            //// return true if both first and second are same reference, or both null
            if (ReferenceEquals(@this, other))
                return true;

            if ((object)@this == null ^ (object)other == null)
                return false;

            return @this.Equals(other);
        }

        public static bool operator !=(PriceDataPoint @this, PriceDataPoint other)
        {
            return !(@this == other);
        }

        #endregion
    }

    public class StrengthStatistics
    {
        public double PriceOverVolume { get; set; }

        public double ClosePosition { get; set; }

        public double MedianPrice { get; set; }
    }



    public class DataRangePartition : IEquatable<DataRangePartition>
    {
        private List<PriceDataPoint> _data;
        private DataRangePartition _previous;
        public DataRangePartition(DataRangePartition previous)
        {
            _previous = previous;
            _data = new List<PriceDataPoint>();
            Direction = TrendDirection.None;
        }

        public IEnumerable<PriceDataPoint> DataRange { get { return _data; } }

        public void AddData(PriceDataPoint data)
        {
            // if (!_data.Contains(data))
            if (_data.Count == 0 || !_data.Last().Equals(data))
                _data.Add(data);
        }

        public void AddFromLowestPoint(List<PriceDataPoint> list, int start, int count)
        {
            var subList = list.GetRange(start, count);
            double min = subList.Min(price => price.Close);
            int minindex = subList.FindIndex(dataPoint => dataPoint.Close == min);
            for (int i = start + minindex; i < start + count; i++)
                _data.Add(list[i]);
        }

        public static DataRangePartition Create(DataRangePartition previous)
        {
            return new DataRangePartition(previous);
        }

        public TrendDirection Direction { get; set; }

        public DataRangePartition Previous { get { return _previous; } set { _previous = value; } }

        public int Count { get { return _data.Count; } }

        public double PriceRange
        {
            get { return (_data.Last().Close - _data.First().Open); }
        }

        public double PriceRangePercent
        {
            get { return PriceRange / _data.First().Open; }
        }

        public double AverageDailyStrength 
        {
            get
            {
                double first, last, strength;
                if (_data.Count > 1)
                {
                    first = _data.First().Open;
                    last = _data.Last().Close;
                    strength = (last - first) / Count;
                }
                else
                    strength = _data.First().Close - _data.First().Open;

                return strength;
            }
        }

        public double StDevStrength
        {
            get
            {
                var adjustPriceArray = _data.Select(point => point.Close).ToArray();
               // var stDev = MyHelper.CalculateStdDev(adjustPriceArray, AverageDailyStrength);
                var stDev = MyHelper.CalculateStdDev(adjustPriceArray);
                return stDev;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return Equals(obj as DataRangePartition);
        }

        public bool Equals(DataRangePartition other)
        {
            if (other == null)
                return false;

            if (this.Count != other.Count)
                return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (this._data[i] != other._data[i])
                    return false;
            }
            return true;
        }
    }

    public class PriceStatisticsAggregate
    {
        private List<PriceDataPoint> _dataRange;
        private List<DataRangePartition> _partitions;
        private int _slideWindow;
        private string _ticker;
        private CompanyStatisticsAggregate _companyinfo;
        public PriceStatisticsAggregate(string ticker, IEnumerable<PriceDataPoint> range)
        {
            var setting = new YahooCompanyStatisticsSetting(ticker);
            var dl = new YahooCompanyStatisticsDownloader(setting);
            _companyinfo = dl.DownloadObjectTaskAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            _ticker = ticker;
            _dataRange = range.ToList<PriceDataPoint>();
            _partitions = new List<DataRangePartition>(10);
            _slideWindow = 4;
        }

        public string Ticker { get { return _ticker;  } }

        public IEnumerable<DataRangePartition> Partitions
        {
            get { return _partitions; }
        }

        public int SlideWindow { get { return _slideWindow; } set { _slideWindow = value; } }

        public int MoveNext(int index)
        {
            int nextindex = index + 1;
            if (_dataRange.Count - 1 >= nextindex)
                return nextindex;
            else
                return -1;
        }

        private List<int> GetNextSlideWindow(int index, int slide)
        {
            var list = new List<int>(slide);
            var count = 0;
            for (int i = index; i < _dataRange.Count; count++, i++)
            {
                if (count == slide)
                    break;
                list.Add(i);
            }
            return list;
        }

        private List<int> FindMaxAndMin(int start, int end)
        {
            //int end = start + count ;
            var list = new List<int>();
            double min = double.MaxValue, max = double.MinValue;
            int minIndex = -1, maxIndex = -1;
            for (int i = start; i <= end; i++)
            {
                if (min > _dataRange[i].Close)
                {
                    min = _dataRange[i].Close;
                    minIndex = i;
                }

                if (max < _dataRange[i].Close)
                {
                    max = _dataRange[i].Close;
                    maxIndex = i;
                }
            }
            list.Add(minIndex);
            list.Add(maxIndex);
            return list;
        }

        public void Partition()
        {
            int start = 0;
            List<int> slide;
            List<int> maxmin;
            DataRangePartition partition = GetPartition();
            while (start < _dataRange.Count)
            {
                slide = GetNextSlideWindow(start, SlideWindow);
                maxmin = FindMaxAndMin(slide.First(), slide.Last());
                //step 1: find direction
                if (_dataRange[slide.LastOrDefault()].Close >= _dataRange[slide.FirstOrDefault()].Close)
                {
                    partition.Direction = TrendDirection.Up;
                    while (start != maxmin.Last())
                    {
                        for (int i = slide.First(); i <= maxmin.Last(); i++)
                            partition.AddData(_dataRange[i]);
                        start = maxmin.Last();
                        slide = GetNextSlideWindow(start, SlideWindow);
                        maxmin = FindMaxAndMin(slide.First(), slide.Last());
                    }

                    if (slide.Count == 1)
                        partition.AddData(_dataRange[start]);

                    start = maxmin.Last() + 1;
                    if (start < _dataRange.Count)
                        partition = GetPartition();

                    // _dataRange.Where((data, index) => index < slide.LastOrDefault() && index > slide.FirstOrDefault())
                    //     .Max(data => data.CloseAdjusted);
                }
                else
                {
                    partition.Direction = TrendDirection.Down;
                    while (start != maxmin.First())
                    {
                        for (int i = slide.First(); i <= maxmin.First(); i++)
                            partition.AddData(_dataRange[i]);
                        start = maxmin.First();
                        slide = GetNextSlideWindow(start, SlideWindow);
                        maxmin = FindMaxAndMin(slide.First(), slide.Last());
                    }

                    if (slide.Count == 1)
                        partition.AddData(_dataRange[start]);

                    start = maxmin.First() + 1;
                    if (start < _dataRange.Count)
                        partition = GetPartition();
                }
            }
        }

        public void RunPartitionAnalysis()
        {
            foreach (var partition in Partitions)
            {
                if (Math.Abs(partition.PriceRangePercent) <= 0.01)
                    partition.Direction = TrendDirection.Range;
                if (partition.AverageDailyStrength < 0 && partition.Direction == TrendDirection.Up ||
                   partition.AverageDailyStrength > 0 && partition.Direction == TrendDirection.Down)
                    partition.Direction = TrendDirection.Range;
                
            }
        }

        public DataRangePartition GetPartition(int index = int.MaxValue)
        {
            if (index < _partitions.Count)
                return _partitions[index];
            else
            {
                var last = _partitions.LastOrDefault();
                _partitions.Add(DataRangePartition.Create(last));
                return _partitions.LastOrDefault();
            }
        }

        public double UpdayAverageGain
        {
            get
            {
                var count = _partitions.Count(p => p.Direction == TrendDirection.Up);
                var val = _partitions.Where(p => p.Direction == TrendDirection.Up).Sum(p => p.AverageDailyStrength);
                return val / count;
            }
        }

        public double DowndayAverageGain
        {
            get
            {
                var count = _partitions.Count(p => p.Direction == TrendDirection.Down);
                var val = _partitions.Where(p => p.Direction == TrendDirection.Down).Sum(p => p.AverageDailyStrength);
                return val / count;
            }
        }

        public DataRangePartition MaxUpPercent
        {
            get 
            {
                var val = from partition in _partitions
                          let largestGain = _partitions.Max(p => p.PriceRange)
                          where partition.Direction == TrendDirection.Up &&
                          partition.PriceRange == largestGain
                          select partition;
                return val.First();
              //  return _partitions.Where(partition => partition.Direction == TrendDirection.Up).Max(partition => partition.PriceRange); 
            }            
        }

        public DataRangePartition MaxDownPercent
        {
            get
            {
                var val = from partition in _partitions
                          let largestLoss = _partitions.Min(p => p.PriceRange)
                          where partition.Direction == TrendDirection.Down &&
                          partition.PriceRange == largestLoss
                          select partition;
                return val.First();
                //  return _partitions.Where(partition => partition.Direction == TrendDirection.Up).Max(partition => partition.PriceRange); 
            }         
        }

    }

}
