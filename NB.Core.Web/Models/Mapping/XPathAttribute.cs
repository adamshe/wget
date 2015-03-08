using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models.Metadata
{
    [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class XPathAttribute : Attribute
    {
        string _path;
        string _name;
        string _source;
        string _regex;
        public XPathAttribute(string path, string name="", string source="", string regex = "")
        {
            _path = path;
            _name = name;
            _source = source;
            _regex = regex;
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

        public string RegexExpression
        {
            get { return _regex; }
            set { _regex = value; }
        }

    }
}
