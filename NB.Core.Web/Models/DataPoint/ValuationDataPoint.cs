using NB.Core.Web.Models.Enums;
using System.ComponentModel;
using NB.Core.Web.Models.Metadata;

namespace NB.Core.Web.Models
{

    public class StockDataPointAggregate
    {
        public StockDataPointAggregate(string ticker)
        {
            Ticker = ticker;
        }

        public StockDataPointAggregate(ValuationDataPoint self, ValuationDataPoint sector, ValuationDataPoint industry)
        {
            Self = self;
            Sector = sector;
            Industry = industry;
        }
        public ValuationDataPoint Self { get; set; }

        public ValuationDataPoint Sector { get; set; }

        public ValuationDataPoint Industry { get; set; }

        public string Ticker { get; set; }
    }

    public class SectorValuationDataPointAggregate
    {
        public SectorValuationDataPointAggregate(ValuationDataPoint data=null)
        {
            Sector = data;
        }

        public SectorValuationDataPointAggregate(ValuationDataPoint[] datapoints)
        {
            Industries = datapoints;
        }

        public ValuationDataPoint Sector { get; set; }

        public ValuationDataPoint[] Industries { get; set; }
    }

    public class ValuationDataPoint
    {
        [XPath("/td[1]/font[2]/a", Name = "Sector", Source="href", RegexExpression = @"(\d*)\w*\.html$")]
        [XPath("/td[1]/font[2]/font/a", Name = "Industry", Source="href", RegexExpression = @"(\d*)\w*\.html$")]
        public int ID { get; set; }

        //[XPath("td[1]/font[1]/b",  RegexExpression = @"(\w+):$")]
        public ContextType Context { get; set; }

        [XPath("/td[1]/font[2]/a", Name = "Sector")]
        [XPath("/td[1]/font[2]", Name = "Industry", RegexExpression = @"(\w+\s+\w+)\s+\(.*\)$")]
        [XPath("/td[1]/font/a[1]", Name ="Equity")]
        public string Description { get; set; }

        [XPath("/td[2]/font")]
        public float PriceChange { get; set; }

         [XPath("/td[3]/font")]
        public string MarketCap { get; set; }

        [XPath("/td[4]/font")]
        public float PE { get; set; }

        [XPath("/td[5]/font")]
        [Description("ROE %")]
        public float ROE { get; set; }

        [XPath("/td[6]/font")]
        [Description("Div Yield %")]
        public float DivYield { get; set; }

        [XPath("/td[7]/font")]
        [Description("LongtermDebtToEquity %")]
        public float LongtermDebtToEquity { get; set; }

        [XPath("/td[8]/font")]
        [Description("Price To Book %")]
        public float PriceToBook { get; set; }

        [XPath("/td[9]/font")]
        [Description("Net Profit Margin %")]
        public float NetProfitMargin { get; set; }

        [XPath("/td[10]/font")]
        [Description("Price to cash flow %")]
        public float PriceToCashFlow { get; set; }
    }
}
