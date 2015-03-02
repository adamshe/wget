using NB.Core.Web.Models.Metadata;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web;

namespace NB.Core.Web.Models
{
    [DataContract]
    public class FinvizCompanyDetails
    {
        string _indexName;
        #region FA

        public string Ticker { get; set; }
       ///table//html/body/table[3]/tbody/tr[1]/td/table/tbody/tr[6]/td/table/tbody/tr[1]/td[1]
        [XPath(@"/table/tr[1]/td[2]/b")]
        public string IndexName { 
            get { return _indexName; } 
            
            set { _indexName = HttpUtility.HtmlDecode(value); } }

        [XPath(@"/table/tr[2]/td[2]/b")]
        public string MarketCap { get; set; }

        [XPath(@"/table/tr[3]/td[2]/b")]
        public string Income { get; set; }

        [XPath(@"/table/tr[4]/td[2]/b")]
        public string Sales { get; set; }

        [XPath(@"/table/tr[5]/td[2]/b")]
        public float BookPerShare { get; set; }

        [XPath(@"/table/tr[6]/td[2]/b")]
        public float CashPerShare { get; set; }

        [XPath(@"/table/tr[7]/td[2]/b")]
        public double? Dividend { get; set; }

        [XPath(@"/table/tr[8]/td[2]/b")]
        public double? DividendPercent { get; set; }

        [XPath(@"/table/tr[9]/td[2]/b")]
        public int Employ { get; set; }

        [XPath(@"/table/tr[10]/td[2]/b")]
        public string Optionable { get; set; }

        [XPath(@"/table/tr[11]/td[2]/b")]
        public string Shortable { get; set; }
        
        [XPath(@"/table/tr[12]/td[2]/b")]
        public float BuyRecommendaton { get; set; }

        [XPath(@"/table/tr[1]/td[4]/b")]
        public float PE { get; set; }

        [XPath(@"/table/tr[2]/td[4]/b")]
        public float ForwardPE { get; set; }

        [XPath(@"/table/tr[3]/td[4]/b")]
        public float PEG { get; set; }

        [XPath(@"/table/tr[4]/td[4]/b")]
        [Description("Price to cash per share Trailing Twelve Month")]
        public float PS { get; set; }

        [XPath(@"/table/tr[5]/td[4]/b")]
        [Description("Price to book per share Most Recent Quarter")]
        public float PB { get; set; }

        [XPath(@"/table/tr[6]/td[4]/b")]
        [Description("Price to cash per share MRQ")]
        public float PC { get; set; }

        [XPath(@"/table/tr[7]/td[4]/b")]
        [Description("Price to Cash Flow TTM")]
        public float PFCF { get; set; }

        [XPath(@"/table/tr[8]/td[4]/b")]
        public float QuickRatio { get; set; }

        [XPath(@"/table/tr[9]/td[4]/b")]
        public float CurrentRatio { get; set; }

        [XPath(@"/table/tr[10]/td[4]/b")]
        public float TotalDebtEquityRatio { get; set; }

        [XPath(@"/table/tr[11]/td[4]/b")]
        public float LongtermDebtEquityRatio { get; set; }

        [XPath(@"/table/tr[11]/td[6]/b")]
        public DateTime EarningDate { get; set; }

        [XPath(@"/table/tr[1]/td[8]/b")]
        public float? InsiderOwnership { get; set; }

        [XPath(@"/table/tr[2]/td[8]/b")]
        public float? InsiderTransaction { get; set; }

        [XPath(@"/table/tr[3]/td[8]/b")]
        public float InstitutionOwnership { get; set; }

        [XPath(@"/table/tr[4]/td[8]/b")]
        public float InstitutionTransaction { get; set; }

        [XPath(@"/table/tr[5]/td[8]/b")]
        [Description("Return on Asset TTM")]
        public float ROA { get; set; }

        [XPath(@"/table/tr[6]/td[8]/b")]
        [Description("Return on Equity TTM")]
        public float ROE { get; set; }

        [XPath(@"/table/tr[7]/td[8]/b")]
        [Description("Return on Investment TTM")]
        public float ROI { get; set; }

        [XPath(@"/table/tr[8]/td[8]/b")]
        public float GrossMarginTTM { get; set; }

        [XPath(@"/table/tr[9]/td[8]/b")]
        public float OperationMarginTTM { get; set; }

        [XPath(@"/table/tr[10]/td[8]/b")]
        public float ProfitMarginTTM { get; set; }

        [XPath(@"/table/tr[11]/td[8]/b")]
        public float Payout { get; set; }

        [XPath(@"/table/tr[1]/td[10]/b")]
        public string SharesOutstanding { get; set; }

        [XPath(@"/table/tr[2]/td[10]/b")]
        public string SharesFloat { get; set; }

        [XPath(@"/table/tr[3]/td[10]/b")]
        public float ShortFloatPercent { get; set; }

        [XPath(@"/table/tr[4]/td[10]/b")]
        public float ShortRatio { get; set; }

 
        #endregion

        #region Growth Factor

        [XPath(@"/table/tr[1]/td[6]/b")]
        public float EPSTTM { get; set; }

        [XPath(@"/table/tr[2]/td[6]/b")]
        public float EPSNextYear { get; set; }

        [XPath(@"/table/tr[3]/td[6]/b")]
        public float EPSNextQ { get; set; }

        [XPath(@"/table/tr[4]/td[6]/b")]
        public float EPSGrowthThisYear { get; set; }

        [XPath(@"/table/tr[5]/td[6]/b")]
        public float EPSGrowthNextYear { get; set; }

        [XPath(@"/table/tr[6]/td[6]/b")]
        public float EPSGrowthNext5Year { get; set; }

        [XPath(@"/table/tr[7]/td[6]/b")]
        public float EPSGrowthPast5Year { get; set; }

        [XPath(@"/table/tr[8]/td[6]/b")]
        public float SalesGrowthPast5Year { get; set; }

        [XPath(@"/table/tr[9]/td[6]/b")]
        public float QuarterlySalesGrowthYearOverYear { get; set; }

        [XPath(@"/table/tr[10]/td[6]/b")]
        public float QuarterlyEarningGrowthYearOverYear { get; set; }


        #endregion

        #region TA

        [XPath(@"/table/tr[12]/td[4]/b")]
        public float SMA20RelativeTo { get; set; }

        [XPath(@"/table/tr[12]/td[6]/b")]
        public float SMA50RelativeTo { get; set; }

        [XPath(@"/table/tr[12]/td[8]/b")]
        public float SMA200RelativeTo { get; set; }


        [XPath(@"/table/tr[5]/td[10]/b")]
        public float TargetPrice { get; set; }

        [XPath(@"/table/tr[6]/td[10]/b")]
        public string Range52Week { get; set; }

        [XPath(@"/table/tr[7]/td[10]/b")]
        public float OffFromHigh52Week { get; set; }

        [XPath(@"/table/tr[8]/td[10]/b")]
        public float OffFromLOw52Week { get; set; }

        [XPath(@"/table/tr[9]/td[10]/b")]
        public float RSI { get; set; }

        [XPath(@"/table/tr[10]/td[10]/b")]
        public float RelativeVolume { get; set; }

        [XPath(@"/table/tr[11]/td[10]/b")]
        public string AverageVolume { get; set; }

        [XPath(@"/table/tr[12]/td[10]/b")]
        [Description("Current Volume / 3-month Average Volume")]
        public long Volume { get; set; }

        [XPath(@"/table/tr[1]/td[12]/b")]
        public float WeekPerformance { get; set; }

        [XPath(@"/table/tr[2]/td[12]/b")]
        public float MonthPerformance { get; set; }

        [XPath(@"/table/tr[3]/td[12]/b")]
        public float QuarterPerformance { get; set; }

        [XPath(@"/table/tr[4]/td[12]/b")]
        public float HalfYearPerformance { get; set; }

        [XPath(@"/table/tr[5]/td[12]/b")]
        public float YearPerformance { get; set; }

        [XPath(@"/table/tr[6]/td[12]/b")]
        public float YearToDatePerformance { get; set; }

        [XPath(@"/table/tr[7]/td[12]/b")]
        [Description("Volatility relative to stock market")]
        public float Beta { get; set; }

        [XPath(@"/table/tr[8]/td[12]/b")]
        [Description("{14 days true range} = max[({high} - {low}), abs({high} - {closeprev}), abs(low-closeprev)]")]
        public float AverageTrueRange { get; set; }

        [XPath(@"/table/tr[9]/td[12]/b", "Volatility")]
        [RegularExpression(@"(.*%)\s.*%$")]
        public string WeeklyVolatility { get; set; }

        [XPath(@"/table/tr[9]/td[12]/b", "Volatility")]
        [RegularExpression(@".*%\s(.*%)$")]
        public string MonthlyVolatility { get; set; }

        [XPath(@"/table/tr[10]/td[12]/b")]
        public float PreviousClose { get; set; }

        [XPath(@"/table/tr[11]/td[12]/b")]
        public float CurrentPrice { get; set; }

        [XPath(@"/table/tr[12]/td[12]/b")]
        [Description("Change from previous close")]
        public float ChangePercent { get; set; }
        #endregion

    }
}
