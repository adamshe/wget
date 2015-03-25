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
        public CpiDataDownloader(BaseSetting setting)
            : base(setting)
        {

        }

        protected override CpiDataAggregate ConvertResult(string content, string ticker = "")
        {
            List<CpiData> dataList = new List<CpiData>(100);
            XParseDocument doc = MyHelper.GetResultTable(content, 1, "<table");
            XParseElement[] results = XPath.GetElements("//tr", doc);
            XParseElement targetNode = null;
            XPathAttribute xpath;
            object value;
            int rowCount = 0;
            foreach (XParseElement row in results)
            {                
                if (rowCount++ == 0)
                    continue;
                var data = new CpiData();
                foreach (var property in data.GetType().GetProperties())
                {
                    xpath = property.GetCustomAttributes(typeof(XPathAttribute), false).FirstOrDefault() as XPathAttribute;

                    if (xpath == null)
                        continue;

                    targetNode = XPath.GetElement(xpath.Path, row);
                    var val = targetNode.Value;
                    if (!(string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val)))
                    {
                        value = Convert.ChangeType(val, property.PropertyType);
                        property.SetValue(data, value);
                    }
                    else
                        property.SetValue(data, double.NaN);
                }
                dataList.Add(data);
            }

            return new CpiDataAggregate(dataList.ToArray());
        }
    }
}
