using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Interfaces
{
    public interface IResultIndexSettings
    {
        /// <summary>
        /// The results queue start index.
        /// </summary>
        int Index { get; set; }
        /// <summary>
        /// The total number of results.
        /// </summary>
        int Count { get; set; }
    }
}
