using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.Core.Web.DownloadClient;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using NB.Core.Web.DownloadSettings;
using System.Globalization;

namespace NB.Core.Web.DownloadClient
{
    public class FinvizDetailsDownloader : BaseDownloader<FinvizCompanyDetails>
    {
        public FinvizDetailsDownloader(BaseSetting setting)
            : base(setting)
        {

        }

        //http:/table/regexhero.net/tester/
        protected override FinvizCompanyDetails ConvertResult(string contentStr, string ticker = "")
        {
#if DEBUG
            Console.WriteLine(contentStr);
#endif
            var normalizeContent = contentStr.Replace("\r\n", "").Replace("\t", "");
            string startPattern = "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"0\" border=\"0\" class=\"snapshot-table2\">";
            string endPattern = "</table>";
            int startIndex = normalizeContent.IndexOf(startPattern);
            int endIndex = normalizeContent.IndexOf("</table>", startIndex);

            var tableStr = normalizeContent.Substring(startIndex, endPattern.Length + endIndex - startIndex);
            
      //var tableStr = MyHelper.ExtractPattern(normalizeContent, ".*<table.*class=\"snapshot-table2\">(?<table>.*)</table>");
            XParseDocument doc = MyHelper.ParseXmlDocument(tableStr);
            XParseElement resultNode;// = XPath.GetElement("/table/tr[1]/td[2]/b", doc);
            object value = null;
            dynamic normalizeStr;
            XPathAttribute xpath;
            var details = new FinvizCompanyDetails();
            foreach (var property in details.GetType().GetProperties())
            {
                xpath = property.GetCustomAttributes(typeof(XPathAttribute), false).FirstOrDefault() as XPathAttribute;                
                resultNode = XPath.GetElement(xpath.Path, doc);

                var type = Nullable.GetUnderlyingType(property.PropertyType);
                
                normalizeStr = MyHelper.NumerizeString(resultNode.Value);
                
                if (property.PropertyType == typeof(DateTime))
                    normalizeStr = MyHelper.ConvertEarningDate(normalizeStr);

                try
                {
                    if (type != null)
                        value = Convert.ChangeType(normalizeStr, type, MyHelper.DefaultCulture.NumberFormat);
                    else
                        value = Convert.ChangeType(normalizeStr, property.PropertyType);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }

                property.SetValue(details, value);
            }

            return details;
        }
    }
}
