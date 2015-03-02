using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class CompanyStatisticsData
    {
        private string _ticker;
        private CompanyValuationMeasures mValuationMeasures = null;
        private CompanyFinancialHighlights mFinancialHighlights = null;

        private CompanyTradingInfo mTradingInfo = null;
        /// <summary>
        /// The ID of the company.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get { return _ticker; }
        }

        public CompanyValuationMeasures ValuationMeasures
        {
            get { return mValuationMeasures; }
        }
        public CompanyFinancialHighlights FinancialHighlights
        {
            get { return mFinancialHighlights; }
        }
        public CompanyTradingInfo TradingInfo
        {
            get { return mTradingInfo; }
        }

        internal CompanyStatisticsData(string id, CompanyValuationMeasures vm, CompanyFinancialHighlights fh, CompanyTradingInfo ti)
        {
            _ticker = id;
            mValuationMeasures = vm;
            mFinancialHighlights = fh;
            mTradingInfo = ti;
        }
    }
}
