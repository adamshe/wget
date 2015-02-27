using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Enums
{
    public enum CarrierGateWay
    {        
        [Description("txt.att.net")]
        ATT = 0,

        [Description("tmomail.net")]
        TMOBILE = 1
    }
}
