using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NB.Core.Web.Models
{
    [DataContract]
    public class TrefisCompanyCoveredInfoBullishData
    {
        [DataMember(Name="bearPct")]
        public int BearPercent { get; set; }

        [DataMember(Name = "bullPct")]
        public int BullPercent { get; set; }

        [DataMember(Name = "capPct")]
        public int CommunityBullPercent { get; set; }

        [DataMember(Name="companyLink")]
        public string CompanyLInk { get; set; }
    }

    [DataContract]
    public class TrefisCompanyCoveredInfoData
    {
        [DataMember(Name="name")]
        public string CompanyName { get; set; }

        public string Ticker
        {
            get
            {
                return MyHelper.ExtractPattern(NumBullish.CompanyLInk, @".*\?hm=(?<ticker>\^?\w*).trefis$");
            }
        }

        [DataMember(Name = "trefis")]
        public float TrefisTarget { get; set; }
        
        public float PriceGap { get {return ( MarketPrice - TrefisTarget) / MarketPrice;} }

        [DataMember(Name="market")]
        public float MarketPrice { get; set; }

        public string Sector { get; set; }
        
        public string Industry { get; set; }

        [DataMember(Name = "numBullish")]
        public TrefisCompanyCoveredInfoBullishData NumBullish { get; set; }
    }

    public class TrefisCompanyCoveredInfoResult
    {
        private TrefisCompanyCoveredInfoData[] mItems = null;
        public TrefisCompanyCoveredInfoData[] Items
        {
            get { return mItems; }
        }
        internal TrefisCompanyCoveredInfoResult(TrefisCompanyCoveredInfoData[] items)
        {
            mItems = items;
        }
    }
}
