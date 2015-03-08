using NB.Core.Valuation;

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

    public class CompanyStatisticsAggregate
    {

        private CompanyStatisticsData mItem = null;
        public CompanyStatisticsData Item
        {
            get { return mItem; }
        }

        internal CompanyStatisticsAggregate(CompanyStatisticsData item)
        {
            mItem = item;
        }

        public double FairMarketValue
        {
            get
            {
                var inflation = 0.02;
                var fixincomeReturnRate = 0.0795;
                var ti = Item.TradingInfo;
                var vm = Item.ValuationMeasures;
                var highlight = Item.FinancialHighlights;
                var eps = Item.FinancialHighlights.DilutedEPS;

                // var growthRate1 = vm.TrailingPE / vm.ForwardPE;
                var growthRate = highlight.QuarterlyRevenueGrowthPercent;//.QuaterlyEarningsGrowthPercent /100.0;
                //var outStandingShare = vm.MarketCapitalisationInMillion / highlight.RevenuePerShare.

                var fairValue = FairValueEngine.DiscountedCurrentValue(eps, 3, growthRate / 100.0, inflation, fixincomeReturnRate);
                if (eps <= 0 && fairValue <= 0)
                    fairValue = FairValueEngine.FutureValue(highlight.RevenuePerShare, growthRate / 100.0, 1) * 1.5;

                return fairValue;
            }
        }
    }
}
