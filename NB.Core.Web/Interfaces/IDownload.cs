using NB.Core.Web.DownloadSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Interfaces
{
    public interface IDownload<T>
    {
        Task<T> DownloadObjectTaskAsync(BaseSetting setting);
        //default interface with setting
        Task<FileInfo> DownloadFileTaskAsync();
        Task<T> DownloadObjectTaskAsync();

        //univeral interface can be used without setting
        Task<T> DownloadObjectTaskAsync(string url);        
        Task<FileInfo> DownloadFileTaskAsync(string url, string fileName);
        Task<string> DownloadStringTaskAync(string url);
        BaseSetting Setting { get; set; }
    }

    public interface IBatchDownload <T>
    {
        Task<IEnumerable<FileInfo>> BatchDownloadFilesTaskAsync(IEnumerable<string> urls);
        Task<IEnumerable<T>> BatchDownloadObjectsTaskAsync (IEnumerable<string> urls);
    }

    public interface IPostDownLoad<T>
    {
        Task<T> PostDownload(Uri url, Dictionary<string, string> data);
        Task<FileInfo> PostDownloadFile(Uri url, Dictionary<string, string> data, string fileName);
    }
}
