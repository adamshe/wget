using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models.Metadata
{
    [AttributeUsage(System.AttributeTargets.Property)]
    public class XPathAttribute : Attribute
    {
        string _path;
        string _name;
        string _source;

        public XPathAttribute(string path, string name="", string source="")
        {
            _path = path;
            _name = name;
            _source = source;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

    }
}
