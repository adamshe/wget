using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Enums;
using NB.Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Interfaces
{
    interface IValuationQuery
    {
        Task<PotentialTarget> GetFairValue(string ticker);       
    }
}
