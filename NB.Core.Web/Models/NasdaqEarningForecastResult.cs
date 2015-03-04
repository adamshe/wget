using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class NasdaqEarningForecastResult
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

        public NasdaqEarningForecastResult(NasdaqEarningForecastData[] yearly, NasdaqEarningForecastData[] quarterly)
        {
            m_yearlyEarningForecasts = yearly;
            m_quarterlyEarningForecasts = quarterly;
        }
    }

    public class NasdaqEarningForecastData 
    {
        string m_fiscalEnd;
        float m_consensusEpsForecast;
        float m_highEpsForecast;
        float m_lowEpsForecast;
        int numberOfEstimate;

        public string FiscalEnd
        {
            get { return m_fiscalEnd; }
            set { m_fiscalEnd = value; }
        }

        public float ConsensusEpsForecast
        {
            get { return m_consensusEpsForecast; }
            set { m_consensusEpsForecast = value; }
        }

        public float HighEpsForecast
        {
            get { return m_highEpsForecast; }
            set { m_highEpsForecast = value; }
        }

        public float LowEpsForecast
        {
            get { return m_lowEpsForecast; }
            set { m_lowEpsForecast = value; }
        }

        public int NumberOfEstimate
        {
            get { return numberOfEstimate; }
            set { numberOfEstimate = value; }
        }
        /// <summary>
        /// Over the last 4 weeks Number of Revision Up
        /// </summary>
        int numOfRevisionUp;

        public int NumOfRevisionUp
        {
            get { return numOfRevisionUp; }
            set { numOfRevisionUp = value; }
        }

        /// <summary>
        /// /// Over the last 4 weeks Number of Revision Down
        /// </summary>
        int numOfrevisionDown;

        public int NumOfrevisionDown
        {
            get { return numOfrevisionDown; }
            set { numOfrevisionDown = value; }
        }

        public string Ticker
        {
            get;
            set;
        }
    }
}
