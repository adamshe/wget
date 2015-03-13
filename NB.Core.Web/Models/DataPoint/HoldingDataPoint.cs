using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.Core.Web.Enums;
using NB.Core.Web.Models.Metadata;
namespace NB.Core.Web.Models.DataPoint
{
    public class HoldingDataPointAggregate
    {
        public HoldingDataPointAggregate(string ticker)
        {
            Ticker = ticker;
        }

        public string Ticker { get; set; }

        private HoldingDataPoint[] _topHolders;

        [XPath("/tr[1]/td")] ////*[@id="form2"]/div[4]/div/table/tbody/tr[1]/td
        public float InstitutionalOwnerShip
        {
            get;set;
        }

        [XPath("/tr[2]/td")]
        public float TotalSharesOutstanding ////*[@id="form2"]/div[4]/div/table/tbody/tr[2]/td
        {
            get;
            set;
        }

        [XPath("/tr[3]/td")]
        public float TotalHoldingValue////*[@id="form2"]/div[4]/div/table/tbody/tr[3]/td
        {
            get;
            set;
        }

        public HoldingDataPoint[] TopHolders
        {
            get { return _topHolders; }
            set { _topHolders = value; }
        }

        //*[@id="form2"]/div[7]/div/table/tbody/tr[2]/td[1]
        [XPath("/div[7]/div/table/tbody/tr[2]/td[1]")]
        public int IncreasePosition { get; set; }
        [XPath("/div[7]/div/table/tbody/tr[2]/td[2]")]
        public int IncreaseShare { get; set; }

        [XPath("/div[7]/div/table/tbody/tr[3]/td[1]")]
        public int DecreasedPosition { get; set; }
        [XPath("/div[7]/div/table/tbody/tr[3]/td[1]")]
        public int DecreasedShare { get; set; }

        [XPath("/div[7]/div/table/tbody/tr[4]/td[1]")]
        public int HeldPosition { get; set; }
        [XPath("/div[7]/div/table/tbody/tr[4]/td[1]")]
        public int HeldShare { get; set; }

        [XPath("/div[10]/div/table/tbody/tr[2]/td[1]")]
        public int NewPosition { get; set; }
        [XPath("/div[10]/div/table/tbody/tr[2]/td[2]")]
        public int NewAddedShare { get; set; }

        [XPath("/div[10]/div/table/tbody/tr[3]/td[1]")]
        public int SoldOutPosition { get; set; }
        [XPath("/div[10]/div/table/tbody/tr[3]/td[2]")]
        public int SoldOutShare { get; set; }
    }

    public class HoldingDataPoint
    {

        public Institution Holder { get; set; }

        [DataType(DataType.Date)]
        [XPath("/td[2]")]
        public DateTime Date { get; set; }

        [XPath("/td[3]")]
        public float ShareOnHand { get; set; }

        [XPath("/td[4]")]
        public float ShareChange { get; set; }

        [XPath("/td[5]")]
        public float ChangePercent { get; set; }

        [XPath("/td[6]")]
        public float ChangeInValue { get; set; }
       
    }

    public class PositionChangePoint
    {
        public PositionChangeType ChangeType { get; set; }
        public int Holders { get; set; }
        public long Shares { get; set; }
    }
}
