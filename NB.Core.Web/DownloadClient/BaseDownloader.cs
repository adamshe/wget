using NB.Core.Web.Interfaces;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NB.Core.Web.Utility;
using System.Collections.Generic;
using NB.Core.Web.DownloadSettings;
using System.Text.RegularExpressions;

namespace NB.Core.Web.DownloadClient
{
    public abstract class BaseDownloader<T> : IDownload<T>, IBatchDownload<T>
    {
        static Lazy<HttpClient> _httpClient = new Lazy<HttpClient>();
        string _fileName = string.Empty;
        BaseSetting _setting;

        public BaseDownloader(BaseSetting setting)
        {
            _setting = setting;
        }

        public async Task<T> DownloadObjectTaskAsync()
        {
            string url = _setting.GetUrlInternal();
            var contentStr = await DownloadStringTaskAync(url);
            T obj = ConvertResult(contentStr, _setting.Ticker);
            return obj;
        }

        public async Task<T> DownloadObjectTaskAsync(string url)
        {
            var contentStr = await DownloadStringTaskAync(url);
            var ticker = _setting.GetTickerFromUrl(url);
            T obj = ConvertResult(contentStr, ticker);
            return obj;
        }

        protected abstract T ConvertResult(string contentStr, string ticker="");

        public async Task<T> DownloadObjectTaskAsync(BaseSetting setting)
        {
            return await DownloadObjectTaskAsync(setting.GetUrlInternal());
        }

        public async Task<string> DownloadStringTaskAync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var contentString = await (await _httpClient.Value.SendAsync(request)).Content.ReadAsStringAsync();

            return contentString;
        }

        public async Task<FileInfo> DownloadFileTaskAsync()
        {
            string url = _setting.GetUrlInternal();
            string fileName = FileName;
            return await DownloadFileTaskAsync(url, fileName);
        }

        public async virtual Task<FileInfo> DownloadFileTaskAsync(string url, string fileName="")
        {
            var ticker = _setting.GetTickerFromUrl(url);
            fileName = fileName == string.Empty ? _setting.GetFileName(ticker.TrimStart(new char[] {'^'})) : fileName;
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var contentStream = await (await _httpClient.Value.SendAsync(request)).Content.ReadAsStreamAsync();
            
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await contentStream.CopyToAsync(fileStream);
                
            }
            return new FileInfo (fileName);            
        }
        

        public string FileName 
        {
            get {
                if (_fileName == string.Empty)
                    return _setting.GetFileName();
                return _fileName;
            }
            set {
                _fileName = value;
            }
        }

        public BaseSetting Setting { get { return _setting; } set { _setting = value; } }

        public int ParallelCapacity { get { return Environment.ProcessorCount * 2; } }

        #region Batch Download

        public async Task<IEnumerable<FileInfo>> BatchDownloadFilesTaskAsync(IEnumerable<string> urls)
        {
            if (urls == null)
                throw new ArgumentNullException("urls is null");
            return await urls.ForEachAsync(ParallelCapacity, url => DownloadFileTaskAsync(url));
        }

        public async Task<IEnumerable<T>> BatchDownloadObjectsTaskAsync(IEnumerable<string> urls)
        {            
            if (urls == null)
                throw new ArgumentNullException("urls is null");
            IEnumerable<T> output = await urls.ForEachAsync<string, T>(ParallelCapacity, url => DownloadObjectTaskAsync(url));
            return output;
        }

        #endregion
    }
}
