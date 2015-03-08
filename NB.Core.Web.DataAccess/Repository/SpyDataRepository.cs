using NB.Core.Web.Enums;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NB.Core.Web.DataAccess.Repository
{
    //http://www.multpl.com/s-p-500-price/
    public class SpyDataRepository
    {
        public SPYValuationDataPointAggregate GetByDate(DateTime date, ValuationType valType)
        {
            var result = new SPYValuationDataPointAggregate(valType);
            return result;
        }

        public void Save (string filePath=@"c:\temp\spy.xml", params SPYValuationDataPointAggregate[] data )
        {
            var doc = CreateXmlFromObjects("root", data);
            doc.Save(filePath);
        }

        public SPYValuationDataPointAggregate[] Load(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var list = new List<SPYValuationDataPointAggregate>(3);
            foreach (var element in doc.Root.Elements("Result"))
            {
                var reader = element.CreateReader();
                var data = new XmlSerializer(typeof(SPYValuationDataPointAggregate)).Deserialize(reader) as SPYValuationDataPointAggregate;
                list.Add(data);
            }
            return list.ToArray();
        }

        private static XDocument CreateXmlFromObjects(string rootElementName, params object[] inputs)
        {
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            
            using (var writer = doc.CreateWriter())
            {
                writer.WriteStartElement(rootElementName);
                foreach (var input in inputs)
                    new XmlSerializer(input.GetType()).Serialize(writer, input);
                writer.WriteEndElement();
            }

            return doc;
        }
    }
}
