using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Enums
{
    public enum ValuationType
    {
        [Description("Price Earning Ratio")]
        PE = 0,

        [Description("Price Sales Ratio")]
        PS,

        [Description("Price Book Ratio")]
        PB
    }
}
