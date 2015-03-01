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
        public XPathAttribute(string path)
        {
            _path = path;
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

    }
}
