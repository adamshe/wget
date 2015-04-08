using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.IO;
using NB.Core.Web.Models.Metadata;
namespace NB.Core.Web.DownloadClient
{
    public class CpiDataDownloader : BaseDownloader<CpiDataAggregate>
    {
        public CpiDataDownloader(CpiDataSetting setting)
            : base(setting)
        {

        }

        protected override CpiDataAggregate ConvertResult(string content, string ticker = "")
        {
            //List<CpiData> dataList = new List<CpiData>(100);
            XParseDocument doc = MyHelper.GetResultTable(content, 1, "<table");
            XParseElement[] results = XPath.GetElements("//tr", doc);
            CpiData[] data = GetResult<CpiData>(results);            
            return GetAggregate(data, _ => new CpiDataAggregate(data));
        }
    }
}
