using NB.Core.Valuation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroOrm.Pocos.SqlGenerator.Attributes;
namespace NB.Core.Web.Models
{
    public class NasdaqEarningForecastAggregate
    {
        NasdaqEarningForecastData[] m_yearlyEarningForecasts;

        public NasdaqEarningForecastData[] YearlyEarningForecasts
        {
            get { return m_yearlyEarningForecasts; }
            set { m_yearlyEarningForecasts = value; }
        }
        NasdaqEarningForecastData[] m_quarterlyEarningForecasts;

        public NasdaqEarningForecastData[] QuarterlyEarningForecasts
        {
            get { return m_quarterlyEarningForecasts; }
            set { m_quarterlyEarningForecasts = value; }
        }

        public NasdaqEarningForecastAggregate(NasdaqEarningForecastData[] yearly, NasdaqEarningForecastData[] quarterly, string ticker)
        {
            m_yearlyEarningForecasts = yearly;
            m_quarterlyEarningForecasts = quarterly;
            Ticker = ticker;
        }

        public string Ticker { get; set; }

        public double QuartylyEarningGrowth
        {
            get
            {
                return GetGrowthRate(m_quarterlyEarningForecasts);
            }
        }

        public double YearlyEarningGrowth
        {
            get
            {
                return GetGrowthRate(m_yearlyEarningForecasts);
            }
        }

        public double FutureFairPEGPrice
        {
            get
            {
                var futureEaring = m_yearlyEarningForecasts.Last().ConsensusEpsForecast;
                var pe = Math.Max(15, Math.Min (30 , 100 * YearlyEarningGrowth)); // peg = 1

                var projection =  pe * futureEaring;
                return projection < 0 ? double.NaN : projection;
            }
        }

        public double CurrentFairPEGPrice
        {
            get
            {
                if (FutureFairPEGPrice == double.NaN)
                    return double.NaN;

                var growthRate = Math.Min (20, GetGrowthRate (m_yearlyEarningForecasts));
                var fmv= FutureFairPEGPrice / Math.Pow(1+growthRate,Years);
                return fmv;
            }
        }

        public int Years { get { return m_yearlyEarningForecasts.Length; } }

        private double GetGrowthRate (NasdaqEarningForecastData[] data)
        {
            if (data.Length == 0)
                return double.NaN;
            var first = data.First().ConsensusEpsForecast;
            var last = data.Last().ConsensusEpsForecast;
            var howManyYears = data.Length;
            double totalGrowth, val;
            if (first < 0 || last < 0)
            {
                totalGrowth = last - first;
                val = totalGrowth / Math.Abs(first) - 1;
            }
            else
            {
                totalGrowth = last / first;


                val = Math.Pow(totalGrowth, 1.0 / howManyYears) - 1;
            }
            return val;
        }

        public override string ToString()
        {
            return string.Format("{0}: Next {1} Q Growth: @{2:p}, Next {3} Year Growth: @{4:p}, Year {5} earning {6},  Current FMV @{7:F2}, Future FMV @{8:F2}",
                Ticker,
                m_quarterlyEarningForecasts.Length, QuartylyEarningGrowth,
                m_yearlyEarningForecasts.Length, YearlyEarningGrowth,
                m_yearlyEarningForecasts.Last().FiscalEnd,
                m_yearlyEarningForecasts.Last().ConsensusEpsForecast,
                CurrentFairPEGPrice,
                FutureFairPEGPrice                
                );
        }
    }

    [StoredAs("EarningForecast")]
    public class NasdaqEarningForecastData : IEqualityComparer<NasdaqEarningForecastData>
    {
        string _fiscalEnd;
        float _consensusEpsForecast;
        float _highEpsForecast;
        float _lowEpsForecast;
        int _numberOfEstimate;
        int _numOfRevisionUp;
        int _numOfrevisionDown;

        [KeyProperty(Identity = true)]
        public int Id { get; set; }

        [StoredAs("Ticker")]
        public string Ticker
        {
            get;
            set;
        }

        [StoredAs("Frequency")]
        public string Frequency { get; set; }

        [StoredAs("FiscalEnd")]
        public string FiscalEnd
        {
            get { return _fiscalEnd; }
            set { _fiscalEnd = value; }
        }

        [StoredAs("Consensus")]
        public float ConsensusEpsForecast
        {
            get { return _consensusEpsForecast; }
            set { _consensusEpsForecast = value; }
        }

        [StoredAs("High")]
        public float HighEpsForecast
        {
            get { return _highEpsForecast; }
            set { _highEpsForecast = value; }
        }

        [StoredAs("Low")]
        public float LowEpsForecast
        {
            get { return _lowEpsForecast; }
            set { _lowEpsForecast = value; }
        }

        [StoredAs("Estimates")]
        public int NumberOfEstimate
        {
            get { return _numberOfEstimate; }
            set { _numberOfEstimate = value; }
        }
        /// <summary>
        /// Over the last 4 weeks Number of Revision Up
        /// </summary>

        [StoredAs("ReviseUps")]
        public int NumOfRevisionUp
        {
            get { return _numOfRevisionUp; }
            set { _numOfRevisionUp = value; }
        }

        /// <summary>
        /// /// Over the last 4 weeks Number of Revision Down
        /// </summary>

        [StoredAs("ReviseDowns")]
        public int NumOfrevisionDown
        {
            get { return _numOfrevisionDown; }
            set { _numOfrevisionDown = value; }
        }

        

        public static bool operator == (NasdaqEarningForecastData a, NasdaqEarningForecastData b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if ((a == null) || (b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(NasdaqEarningForecastData a, NasdaqEarningForecastData b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var comparison = obj as NasdaqEarningForecastData;

            if (comparison == null)
                return false;

            return Equals(comparison);
        }

        public bool Equals (NasdaqEarningForecastData y)
        {
            return this.Ticker == y.Ticker &&
                 this.FiscalEnd == y.FiscalEnd &&
                  this.ConsensusEpsForecast == y.ConsensusEpsForecast &&
                  this.HighEpsForecast == y.HighEpsForecast &&
                  this.LowEpsForecast == y.LowEpsForecast &&
                  this.NumberOfEstimate == y.NumberOfEstimate &&
                  this.NumOfRevisionUp == y.NumOfRevisionUp &&
                  this.NumOfrevisionDown == y.NumOfrevisionDown;
        }
        public bool Equals(NasdaqEarningForecastData x, NasdaqEarningForecastData y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 1;

                // String properties
                hashCode = (hashCode * 397) ^ (Ticker!= null ? Ticker.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FiscalEnd!= null ? FiscalEnd.GetHashCode() : 0);

                // int properties
                hashCode = (hashCode * 397) ^ (int)ConsensusEpsForecast;
                hashCode = (hashCode * 397) ^ (int)HighEpsForecast;
                hashCode = (hashCode * 397) ^ (int)LowEpsForecast;
                hashCode = (hashCode * 397) ^ NumberOfEstimate;
                hashCode = (hashCode * 397) ^ NumOfRevisionUp;
                hashCode = (hashCode * 397) ^ NumOfrevisionDown;
                return hashCode;
            }
        }


        public int GetHashCode(NasdaqEarningForecastData obj)
        {
            return obj.GetHashCode();
        }
    }
}
