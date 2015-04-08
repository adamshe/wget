using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadClient
{
    public class AnalystRatingsDownloader : BaseDownloader<AnalystRatingsDataAggregate>
    {
        public AnalystRatingsDownloader(AnalystRatingsSetting setting) : base(setting)
        {

        }

        protected override AnalystRatingsDataAggregate ConvertResult(string contentStr, string ticker = "")
        {
            XParseDocument doc = MyHelper.GetResultTable(contentStr, 1, @"<table id='ratingstable'");
            XParseElement[] rows = XPath.GetElements("//tr", doc);
            AnalystRatingsData[] data = GetResult<AnalystRatingsData>(rows);
            return GetAggregate(data, _ => new AnalystRatingsDataAggregate(data));
        }
    }
}
