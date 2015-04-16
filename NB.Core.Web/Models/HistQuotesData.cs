using NB.Core.Web.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
	public class HistQuotesData
	{
		/// <summary>
		/// The startdate of the period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public DateTime TradingDate { get; set; }
		/// <summary>
		/// The first value in trading period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double Open { get; set; }
		/// <summary>
		/// The highest value in trading period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double High { get; set; }
		/// <summary>
		/// The lowest value in trading period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double Low { get; set; }
		/// <summary>
		/// The last value in trading period.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double Close { get; set; }
		/// <summary>
		/// The last value in trading period in relation to share splits.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double CloseAdjusted { get; set; }
		/// <summary>
		/// The traded volume.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public long Volume { get; set; }
		/// <summary>
		/// The close value of the previous HistQuoteData in chain.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public double PreviousClose { get; set; }
	}

	public class DataRangePartition
	{
		private List<PriceDataPoint> _data;
		private DataRangePartition _previous;
		public DataRangePartition (DataRangePartition previous)
		{
			_previous = previous;
			_data = new List<PriceDataPoint>();
			Direction = TrendDirection.None;
		}

		public IEnumerable<PriceDataPoint> DataRange { get { return _data; } }

		public void AddData(PriceDataPoint data)
		{
		   // if (!_data.Contains(data))
			if(!_data.Last().Equals(data))
				_data.Add(data);
		}

		public void AddFromLowestPoint(List<PriceDataPoint> list, int start, int count)
		{
			var subList = list.GetRange(start, count);            
			double min = subList.Min(price => price.Adjust);
			int minindex = subList.FindIndex(dataPoint => dataPoint.Adjust == min);
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
	}

	public class PriceStatisticsAggregate
	{
		private List<PriceDataPoint> _dataRange;
		private List<DataRangePartition> _partitions;
		private int _slideWindow;
		public PriceStatisticsAggregate(IEnumerable<PriceDataPoint> range)
		{
			_dataRange = range.ToList<PriceDataPoint>();
			_partitions = new List<DataRangePartition>(10);
			_slideWindow = 4;
		}

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

		private List<int> GetNextSlideWindow (int index, int slide)
		{
			var list = new List<int>(slide);
			var count = 0;
			for (int i = index ; i < _dataRange.Count; count++, i++)
			{
				if (count == slide)
					break;
				list.Add(i);
			}
			return list;
		}

		private List<int> FindMaxAndMin (int start, int end)
		{
			//int end = start + count ;
			var list = new List<int>();
			double min = double.MaxValue, max=double.MinValue;
			int minIndex = -1, maxIndex = -1;
			for (int i = start; i <= end; i++)
			{
				if (min > _dataRange[i].Adjust)
				{
					min = _dataRange[i].Adjust;
					minIndex = i;
				}

				if (max < _dataRange[i].Adjust)
				{
					max = _dataRange[i].Adjust;
					maxIndex = i;
				}
			}
			list.Add(minIndex);
			list.Add(maxIndex);
			return list;
		}
		public void Partition ()
		{
			int start = 0, end, peek, peek2;
			List<int> slide;
			List<int> peekSlide;
			List<int> maxmin;
			List<int> peekMaxmin;
			DataRangePartition partition = GetPartition();
			while (start < _dataRange.Count )
			{                
				slide = GetNextSlideWindow(start, SlideWindow);
				maxmin = FindMaxAndMin(slide.First(), slide.Last());
				//step 1: find direction
				if (_dataRange[slide.LastOrDefault()].Adjust > _dataRange[slide.FirstOrDefault()].Adjust)
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
					start = maxmin.Last() + 1;
					if (start <_dataRange.Count )
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
					
					start = maxmin.First() + 1;
					if ( start <_dataRange.Count )
						partition = GetPartition();
				}				
			}
		}

		public DataRangePartition GetPartition (int index = int.MaxValue)
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

		public double MaxUp
		{
			get;
			set;
		}

		public double MaxDown
		{
			get;
			set;
		}

	}
}
