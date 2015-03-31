﻿using System;
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

    public class PriceDataPoint
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
    }
}
