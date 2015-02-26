// ******************************************************************************
// ** 
// **  MaasOne WebServices
// **  Written by Marius Häusler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius Häusler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************
using System;
using System.Collections.Generic;
using NB.Core.Web.Xml;
using NB.Core.Web.Interfaces;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using NB.Core.Web.Enums;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;

namespace NB.Core.Web.Utility
{
    public static class MyHelper
    {
      //  public static readonly CultureInfo ConverterCulture = new CultureInfo("en-US");
        private static CultureInfo _culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

        public static string StreamToString(Stream stream, System.Text.Encoding encoding = null)
        {
            string res = string.Empty;
            if (stream != null)
            {
                System.Text.Encoding enc = encoding;
                if (enc == null)
                    enc = Encoding.UTF8;

                if (stream.CanSeek) stream.Seek(0, System.IO.SeekOrigin.Begin);
                if (stream.CanRead)
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream, enc))
                    {
                        res = sr.ReadToEnd();
                    }
                }
            }
            return res;
        }
        public static byte[] StreamToBytes(System.IO.Stream s)
        {
            byte[] result = new byte[] { };
            if (s != null)
            {
                result = new byte[Convert.ToInt32(s.Length) + 1];
                s.Read(result, 0, Convert.ToInt32(s.Length));
            }
            return result;
        }

        public static async Task<MemoryStream> CopyStreamTaskAsync(Stream source)
        {
            var copy = new System.IO.MemoryStream();
            if (source != null && source.CanRead)
            {
                byte[] buffer = new byte[2048 + 1];
                await source.CopyToAsync(copy, buffer.Length);                                    
            }
            copy.Position = 0;
            return copy;
        }

        public static MemoryStream CopyStream(Stream source)
        {
            var copy = new System.IO.MemoryStream();
            if (source != null && source.CanRead)
            {
                byte[] buffer = new byte[Convert.ToInt32(2048) + 1];
                while (true)
                {
                    int read = source.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        break;
                    }
                    copy.Write(buffer, 0, read);
                }
            }
            copy.Position = 0;
            return copy;
        }

        public static T[] EnumToArray<T>(IEnumerable<T> values)
        {
            if (values != null)
            {
                if (values is T[])
                {
                    return (T[])values;
                }
                else
                {
                    return new List<T>(values).ToArray();
                }
            }
            else
            {
                return new T[] { };
            }
        }
        public static string CharEnumToString(IEnumerable<char> arr)
        {
            string s = string.Empty;
            if (arr != null)
            {
                foreach (char c in arr)
                {
                    s += c;
                }
            }
            return s;
        }
        public static T GetEnumItemAt<T>(IEnumerable<T> values, int index)
        {
            int cnt = 0;
            foreach (T itm in values)
            {
                if (cnt == index) return itm;
                cnt++;
            }
            return default(T);
        }

        public static object StringToObject(string str, System.Globalization.CultureInfo ci)
        {
            string value = str.Replace("%", "").Replace("\"", "").Replace("<b>", "").Replace("</b>", "").Replace("N/A", "").Trim();
            if (value != string.Empty)
            {
                if (value == "-") { return string.Empty; }
                else if (value.Contains(" - "))
                {
                    String[] values = value.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                    List<object> results = new List<object>();
                    foreach (String v in values)
                    {
                        results.Add(StringToObject(v, ci));
                    }
                    if (results.Count == 0) { return string.Empty; }
                    else if (results.Count == 0) { return results[0]; }
                    else { return results.ToArray(); }
                }
                else
                {
                    double dbl = 0;
                    if (double.TryParse(value, System.Globalization.NumberStyles.Any, ci, out dbl))
                    {
                        return dbl;
                    }
                    else
                    {
                        long lng = 0;
                        if (long.TryParse(value, out lng))
                        {
                            return lng;
                        }
                        else
                        {
                            System.DateTime dte = default(System.DateTime);
                            if (System.DateTime.TryParse(value, ci, System.Globalization.DateTimeStyles.AdjustToUniversal, out dte))
                            {
                                return dte;
                            }
                            else
                            {
                                return value;
                            }
                        }
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public static string ObjectToString(object value, System.Globalization.CultureInfo ci)
        {
            if (value != null)
            {
                if (value is double)
                {
                    return Convert.ToDouble(value).ToString(ci);
                }
                else if (value is System.DateTime)
                {
                    return Convert.ToDateTime(value).ToString(ci);
                }
                else if (value is object[])
                {
                    object[] values = (object[])value;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (object obj in values)
                    {
                        sb.Append(ObjectToString(obj, ci));
                        if (!object.ReferenceEquals(obj, values[values.Length - 1]))
                            sb.Append(" - ");
                    }
                    return sb.ToString();
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static XParseDocument ParseXmlDocument(string text) { return XmlParser.Parse(text); }

        public static XParseDocument ParseXmlDocument(System.IO.Stream xml)
        {
            if (xml != null && xml.CanRead)
            {
                try
                {
                    var contentStr = StreamToString(xml, System.Text.Encoding.UTF8);
                    return ParseXmlDocument(contentStr);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static string GetXmlAttributeValue(XParseElement node, string attName)
        {
            if (node != null)
            {
                XParseAttribute att = node.Attribute(XParseName.Get(attName));
                if (att != null) return att.Value;
            }
            return string.Empty;
        }

        public static string YqlUrl(string statement, bool json)
        {
            string format = "json";
            if (json == false)
                format = "xml";
            return "http://query.yahooapis.com/v1/public/yql?q=" + Uri.EscapeDataString(statement) + "&format=" + format + "&diagnostics=false&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
        }
        public static string YqlUrl(string fields, string table, string whereParam, IResultIndexSettings opt, bool json)
        {
            return YqlUrl(YqlStatement(fields, table, whereParam, opt), json);
        }
        public static string YqlStatement(string fields, string table, string whereParam, IResultIndexSettings opt)
        {
            System.Text.StringBuilder stmt = new System.Text.StringBuilder(60);
            stmt.Append("select ");
            stmt.Append(fields);
            stmt.Append(" from ");
            stmt.Append(table);
            if (opt != null && opt.Count > 0)
            {
                stmt.Append("(");
                stmt.Append(opt.Index.ToString());
                stmt.Append(",");
                stmt.Append(opt.Count.ToString());
                stmt.Append(")");
            }
            if (whereParam.Trim() != string.Empty)
            {
                stmt.Append(" where ");
                stmt.Append(whereParam);
            }
            return stmt.ToString();
        }
        public static string CleanYqlParam(string id)
        {
            return id.Replace("\"", "").Replace("'", "").Trim();
        }
     
        public static string[][] CsvTextToStringTable(string csv, char delimiter)
        {
            string[] rows = csv.Split(new String[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            List<string[]> lst = new List<string[]>();
            foreach (string row in rows)
            {
                if (row.Trim() != string.Empty)
                    lst.Add(CsvRowToStringArray(row.Trim(), delimiter));
            }
            return lst.ToArray();
        }

        public static string[] CsvRowToStringArray(string row, char delimiter, bool withQuoteMarks = true)
        {
            if (withQuoteMarks)
            {
                List<string> lstParts = new List<string>();
                int actualIndex = 0;
                int tempStartIndex = 0;
                bool waitForNextQuoteMark = false;

                while (!(actualIndex == row.Length))
                {
                    if (row[actualIndex] == '\"')
                    {
                        waitForNextQuoteMark = !waitForNextQuoteMark;
                    }
                    else if (row[actualIndex] == delimiter)
                    {
                        if (!waitForNextQuoteMark)
                        {
                            lstParts.Add(ClearCsvString(row.Substring(tempStartIndex, actualIndex - tempStartIndex)));
                            tempStartIndex = actualIndex + 1;
                        }
                    }
                    actualIndex += 1;
                    if (actualIndex == row.Length)
                    {
                        string s = ClearCsvString(row.Substring(tempStartIndex, actualIndex - tempStartIndex));
                        lstParts.Add(s);
                    }
                }
                return lstParts.ToArray();
            }
            else
            {
                return row.Split(delimiter);
            }
        }
        private static string ClearCsvString(string csv)
        {
            if (csv.Length > 0)
            {
                string result = csv;
                if (result.StartsWith("\""))
                    result = result.Substring(1);
                if (result.EndsWith("\""))
                    result = result.Substring(0, result.Length - 1);
                return result.Replace("\"\"", "\"");
            }
            else
            {
                return csv;
            }
        }

        public static CultureInfo DefaultCulture { get { return _culture; } }

        public static DateTime ConvertEarningDate (string dateStr)
        {
            Debug.WriteLine(dateStr);
            TimeSpan hour = new TimeSpan();

            string [] dateElements = dateStr.Split(new char []{' '}, StringSplitOptions.RemoveEmptyEntries);
            if (dateElements.Length == 1)
                return DateTime.MaxValue;

            var constructDateStr = string.Format("{0} {1}, {2}", dateElements[0], dateElements[1], DateTime.Now.Year);
            DateTime date = DateTime.ParseExact(constructDateStr, "MMM dd, yyyy", CultureInfo.InvariantCulture);
            
            if (dateElements.Length>2)
                hour = GetTimeSpan((EarningHour)Enum.Parse(typeof(EarningHour), dateElements[2]));
            return date.Add(hour);            
        }

        static TimeSpan GetTimeSpan(EarningHour hour)
        {
            switch (hour)
            {
                case EarningHour.BMO:
                    return new TimeSpan(8, 0, 0);

                case EarningHour.AMC:
                default:
                    return new TimeSpan(17, 0, 0);                
            }
        }

        public static string FixString (string original)
        {
            string originalFix = original.Replace("\r", string.Empty).Replace("\n", string.Empty);
            return Regex.Replace(originalFix, @"\s+", string.Empty);
        }

        public static string[] GetStringToken (string str, string[] delimiter)
        {
            string fixSymbols = MyHelper.FixString(str);
            var tickers = fixSymbols.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            return tickers;
        }
        //private MyHelper() { }
        //static MyHelper() { }
    }
}
