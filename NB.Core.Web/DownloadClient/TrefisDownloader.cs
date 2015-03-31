using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.IO;
namespace NB.Core.Web.DownloadClient
{
    public class TrefisDownloader : BaseDownloader<TrefisCompanyCoveredInfoAggregate>
    {
        public TrefisDownloader(BaseSetting setting): base(setting)
        {

        }
        
        protected override TrefisCompanyCoveredInfoAggregate ConvertResult(string contentStr, string ticker = "")
        {
            List<TrefisCompanyCoveredInfoData> companies = new List<TrefisCompanyCoveredInfoData>(100);
            var normalizeContent = contentStr.Replace("\r\n", "").Replace("\t", "");
            XParseDocument doc = MyHelper.ParseXmlDocument(normalizeContent);

            int startIndex = normalizeContent.IndexOf("initCompanies(") + "initCompanies(".Length;
            int endIndex = normalizeContent.IndexOf(");});</script>");


            var jsonStr = normalizeContent.Substring(startIndex, endIndex - startIndex);
           //     jsonStr = MyHelper.ExtractPattern(normalizeContent, @".*initCompanies\((?<content>.*)\);\}\);</script>");

              //  var serializer = new DataContractJsonSerializer(typeof(TrefisCompanyCoveredInfoData));// JavaScriptSerializer();

                List<TrefisCompanyCoveredInfoData> dataList = MyHelper.FromJson<List<TrefisCompanyCoveredInfoData>>(jsonStr);
                //serializer..Deserialize<List<TrefisCompanyCoveredInfoData>>(scriptContent);
            //XDocument xdoc = XDocument.Parse(normalizeContent);
            //XElement tempElement = null;
            //var rows = xdoc.XPathSelectElements("//tbody[@class=\"cmpTblBody\")]/tr");
            //foreach (var row in rows)
            //{
            //    var data = new TrefisCompanyCoveredInfoData();
            //    tempElement = row.Document.XPathSelectElement("/td[1]/a");
            //    data.CompanyName = tempElement.Value;

            //    data.Ticker = this.Setting.GetTickerFromUrl(tempElement.Attribute(XName.Get("href")).Value);

            //    tempElement = row.Document.XPathSelectElement("/td[2]/a");
            //    data.TrefisTarget = double.Parse(tempElement.Value, NumberStyles.Currency);

            //    tempElement = row.Document.XPathSelectElement("/td[3]");
            //    data.PriceGap = float.Parse(tempElement.Value);

            //    tempElement = row.Document.XPathSelectElement("/td[4]");
            //    data.Sector = tempElement.Value;

            //    tempElement = row.Document.XPathSelectElement("/td[5]");
            //    data.Industry = tempElement.Value;

            //    tempElement = row.Document.XPathSelectElement("/td[6]");
            //    data.Bearishness = float.Parse(tempElement.Value);

            //    companies.Add(data);
            //}
            var culture = MyHelper.DefaultCulture;
            XParseElement[] results = XPath.GetElements("//tbody[@class=\"cmpTblBody\")]/tr", doc);
            XParseElement tempNode = null;
            foreach (XParseElement row in results)
            {
                tempNode = XPath.GetElement("/td[1]/a", row);
                var data = dataList.Where(company => company.CompanyName == HttpUtility.HtmlDecode(tempNode.Value)).FirstOrDefault();
                tempNode = XPath.GetElement("/td[4]", row);
                data.Sector = tempNode.Value;

                tempNode = XPath.GetElement("/td[5]", row);
                data.Industry = tempNode.Value;

            }

            return new TrefisCompanyCoveredInfoAggregate(dataList.ToArray());
        }
    }
}
