using System;
using System.Linq;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using NB.Core.Web.DownloadSettings;
using System.IO;
using NB.Core.Web.Models.Metadata;
using System.ComponentModel.DataAnnotations;

namespace NB.Core.Web.DownloadClient
{
    public class FinvizDetailsDownloader : BaseDownloader<FinvizCompanyDetails>
    {
        public FinvizDetailsDownloader(FinvizDetailsSetting setting)
            : base(setting)
        {

        }
        
        protected override FinvizCompanyDetails ConvertResult(StreamReader sr, string ticker = "")
        {
            throw new NotImplementedException("stream is only for csv files.");
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
            RegularExpressionAttribute regex;
            var details = new FinvizCompanyDetails { Ticker = Setting.Ticker};
            foreach (var property in details.GetType().GetProperties())
            {
                xpath = property.GetCustomAttributes(typeof(XPathAttribute), false).FirstOrDefault() as XPathAttribute;

                if (xpath == null)
                    continue;

                

                resultNode = XPath.GetElement(xpath.Path, doc);

                var type = Nullable.GetUnderlyingType(property.PropertyType);

                normalizeStr = resultNode.Value;

                regex = property.GetCustomAttributes(typeof(RegularExpressionAttribute), false).FirstOrDefault() as RegularExpressionAttribute;

                try
                {
                    if (regex != null)
                        normalizeStr =MyHelper.NumerizeString( MyHelper.ExtractPattern(normalizeStr, regex.Pattern));
                    else if (property.PropertyType == typeof(DateTime))
                        normalizeStr = MyHelper.ConvertEarningDate(normalizeStr);
                    else if (property.PropertyType != typeof(string))
                        normalizeStr = MyHelper.NumerizeString(resultNode.Value);

                
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
