using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using NB.Core.Web.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadClient
{
    public class YahooCompanyStatisticsDownloader : BaseDownloader<CompanyStatisticsResult>
    {
        public YahooCompanyStatisticsDownloader( BaseSetting setting) : base (setting)
        {

        }

        protected override CompanyStatisticsResult ConvertResult(string contentStr, string ticker = "")
        {
            CompanyStatisticsData result = null;
            XParseDocument doc = MyHelper.ParseXmlDocument(contentStr);
            XParseElement resultNode = XPath.GetElement("//table[@id=\"yfncsumtab\"]/tr[2]", doc);

            if (resultNode != null)
            {

                XParseElement tempNode = null;
                XParseElement vmNode = XPath.GetElement("/td[1]/table[2]/tr/td/table", resultNode);
                double[] vmValues = new double[9];
                if (vmNode != null)
                {
                    tempNode = XPath.GetElement("/tr[1]/td[2]/span", vmNode);
                    if (tempNode != null) vmValues[0] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[2]/td[2]", vmNode);
                    if (tempNode != null) vmValues[1] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[3]/td[2]", vmNode);
                    if (tempNode != null) vmValues[2] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[4]/td[2]", vmNode);
                    if (tempNode != null) vmValues[3] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[5]/td[2]", vmNode);
                    if (tempNode != null) vmValues[4] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[6]/td[2]", vmNode);
                    if (tempNode != null) vmValues[5] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[7]/td[2]", vmNode);
                    if (tempNode != null) vmValues[6] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[8]/td[2]", vmNode);
                    if (tempNode != null) vmValues[7] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[9]/td[2]", vmNode);
                    if (tempNode != null) vmValues[8] = FinanceHelper.ParseToDouble(tempNode.Value);

                }

                CompanyValuationMeasures vm = new CompanyValuationMeasures(vmValues);


                XParseElement fyNode = XPath.GetElement("/td[1]/table[4]/tr/td/table", resultNode);
                XParseElement profitNode = XPath.GetElement("/td[1]/table[5]/tr/td/table", resultNode);
                XParseElement meNode = XPath.GetElement("/td[1]/table[6]/tr/td/table", resultNode);
                XParseElement isNode = XPath.GetElement("/td[1]/table[7]/tr/td/table", resultNode);
                XParseElement bsNode = XPath.GetElement("/td[1]/table[8]/tr/td/table", resultNode);
                XParseElement cfsNode = XPath.GetElement("/td[1]/table[9]/tr/td/table", resultNode);

                DateTime fiscalYEnds = new DateTime();
                DateTime mostRecQutr = new DateTime();
                double[] fhValues = new double[20];

                if (fyNode != null)
                {
                    tempNode = XPath.GetElement("/tr[2]/td[2]", fyNode);
                    if (tempNode != null) fiscalYEnds = FinanceHelper.ParseToDateTime(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[3]/td[2]", fyNode);
                    if (tempNode != null) mostRecQutr = FinanceHelper.ParseToDateTime(tempNode.Value);
                }

                if (profitNode != null)
                {
                    tempNode = XPath.GetElement("/tr[2]/td[2]", profitNode);
                    if (tempNode != null) fhValues[0] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[3]/td[2]", profitNode);
                    if (tempNode != null) fhValues[1] = FinanceHelper.ParseToDouble(tempNode.Value);
                }

                if (meNode != null)
                {
                    tempNode = XPath.GetElement("/tr[2]/td[2]", meNode);
                    if (tempNode != null) fhValues[2] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[3]/td[2]", meNode);
                    if (tempNode != null) fhValues[3] = FinanceHelper.ParseToDouble(tempNode.Value);
                }

                if (isNode != null)
                {
                    tempNode = XPath.GetElement("/tr[2]/td[2]", isNode);
                    if (tempNode != null) fhValues[4] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[3]/td[2]", isNode);
                    if (tempNode != null) fhValues[5] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[4]/td[2]", isNode);
                    if (tempNode != null) fhValues[6] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[5]/td[2]", isNode);
                    if (tempNode != null) fhValues[7] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[6]/td[2]", isNode);
                    if (tempNode != null) fhValues[8] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[7]/td[2]", isNode);
                    if (tempNode != null) fhValues[9] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[8]/td[2]", isNode);
                    if (tempNode != null) fhValues[10] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[9]/td[2]", isNode);
                    if (tempNode != null) fhValues[11] = FinanceHelper.ParseToDouble(tempNode.Value);
                }

                if (bsNode != null)
                {
                    tempNode = XPath.GetElement("/tr[2]/td[2]", bsNode);
                    if (tempNode != null) fhValues[12] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[3]/td[2]", bsNode);
                    if (tempNode != null) fhValues[13] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[4]/td[2]", bsNode);
                    if (tempNode != null) fhValues[14] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[5]/td[2]", bsNode);
                    if (tempNode != null) fhValues[15] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[6]/td[2]", bsNode);
                    if (tempNode != null) fhValues[16] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[7]/td[2]", bsNode);
                    if (tempNode != null) fhValues[17] = FinanceHelper.ParseToDouble(tempNode.Value);

                }

                if (cfsNode != null)
                {
                    tempNode = XPath.GetElement("/tr[2]/td[2]", cfsNode);
                    if (tempNode != null) fhValues[18] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[3]/td[2]", cfsNode);
                    if (tempNode != null) fhValues[19] = FinanceHelper.GetMillionValue(tempNode.Value);
                }

                CompanyFinancialHighlights fh = new CompanyFinancialHighlights(fiscalYEnds, mostRecQutr, fhValues);


                XParseElement sphNode = XPath.GetElement("/td[3]/table[2]/tr/td/table", resultNode);
                XParseElement stNode = XPath.GetElement("/td[3]/table[3]/tr/td/table", resultNode);
                XParseElement dsNode = XPath.GetElement("/td[3]/table[4]/tr/td/table", resultNode);

                double[] ctiValues = new double[23];
                DateTime exDivDate = new DateTime();
                DateTime divDate = new DateTime();
                DateTime splitDate = new DateTime();
                SharesSplitFactor sf = null;

                if (sphNode != null)
                {

                    tempNode = XPath.GetElement("/tr[2]/td[2]", sphNode);
                    if (tempNode != null) ctiValues[0] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[3]/td[2]", sphNode);
                    if (tempNode != null) ctiValues[1] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[4]/td[2]", sphNode);
                    if (tempNode != null) ctiValues[2] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[5]/td[2]", sphNode);
                    if (tempNode != null) ctiValues[3] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[6]/td[2]", sphNode);
                    if (tempNode != null) ctiValues[4] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[7]/td[2]", sphNode);
                    if (tempNode != null) ctiValues[5] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[8]/td[2]", sphNode);
                    if (tempNode != null) ctiValues[6] = FinanceHelper.ParseToDouble(tempNode.Value);

                }


                if (stNode != null)
                {

                    tempNode = XPath.GetElement("/tr[2]/td[2]", stNode);
                    if (tempNode != null) ctiValues[7] = FinanceHelper.ParseToDouble(tempNode.Value) / 1000;

                    tempNode = XPath.GetElement("/tr[3]/td[2]", stNode);
                    if (tempNode != null) ctiValues[8] = FinanceHelper.ParseToDouble(tempNode.Value) / 1000;

                    tempNode = XPath.GetElement("/tr[4]/td[2]", stNode);
                    if (tempNode != null) ctiValues[9] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[5]/td[2]", stNode);
                    if (tempNode != null) ctiValues[10] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[6]/td[2]", stNode);
                    if (tempNode != null) ctiValues[11] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[7]/td[2]", stNode);
                    if (tempNode != null) ctiValues[12] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[8]/td[2]", stNode);
                    if (tempNode != null) ctiValues[13] = FinanceHelper.GetMillionValue(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[9]/td[2]", stNode);
                    if (tempNode != null) ctiValues[14] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[10]/td[2]", stNode);
                    if (tempNode != null) ctiValues[15] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[11]/td[2]", stNode);
                    if (tempNode != null) ctiValues[16] = FinanceHelper.GetMillionValue(tempNode.Value);

                }

                if (dsNode != null)
                {

                    tempNode = XPath.GetElement("/tr[2]/td[2]", dsNode);
                    if (tempNode != null) ctiValues[17] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[3]/td[2]", dsNode);
                    if (tempNode != null) ctiValues[18] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[4]/td[2]", dsNode);
                    if (tempNode != null) ctiValues[19] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[5]/td[2]", dsNode);
                    if (tempNode != null) ctiValues[20] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[6]/td[2]", dsNode);
                    if (tempNode != null) ctiValues[21] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[7]/td[2]", dsNode);
                    if (tempNode != null) ctiValues[22] = FinanceHelper.ParseToDouble(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[8]/td[2]", dsNode);
                    if (tempNode != null) divDate = FinanceHelper.ParseToDateTime(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[9]/td[2]", dsNode);
                    if (tempNode != null) exDivDate = FinanceHelper.ParseToDateTime(tempNode.Value);

                    tempNode = XPath.GetElement("/tr[10]/td[2]", dsNode);
                    if (tempNode != null)
                    {
                        string[] txt = tempNode.Value.Split(':');
                        int from, to;
                        if (int.TryParse(txt[0], out to) && int.TryParse(txt[1], out from))
                        {
                            sf = new SharesSplitFactor(to, from);
                        }
                    }

                    tempNode = XPath.GetElement("/tr[11]/td[2]", dsNode);
                    if (tempNode != null) splitDate = FinanceHelper.ParseToDateTime(tempNode.Value);

                }
                CompanyTradingInfo cti = new CompanyTradingInfo(ctiValues, divDate, exDivDate, splitDate, sf);

                result = new CompanyStatisticsData(ticker, vm, fh, cti);
            }
            return new CompanyStatisticsResult(result);
        }

    }
}
