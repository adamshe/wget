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
using System.Net;
using System.Diagnostics;
using NB.Core.Web.Extensions;
namespace NB.Core.Web.DownloadClient
{
    public abstract class BaseDownloader<T> : IDownload<T>, IBatchDownload<T>, IPostDownLoad<T>
    {
        static Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(() => 
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            var client = new HttpClient(handler);
            return client;
        });
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

        protected async Task<StreamReader> DownloadStreamTaskAync()
        {
            string url = _setting.GetUrlInternal();
            var response = await _httpClient.Value.GetStreamAsync(url);
            var reader = new StreamReader(response);

            return reader;
        }

        public async Task<T> DownloadObjectStreamTaskAsync()
        {
            string url = _setting.GetUrlInternal();
            return await DownloadObjectStreamTaskAsync(url);
        }            

        public async Task<T> DownloadObjectStreamTaskAsync(string url)
        {
            var sr = await DownloadStreamTaskAync(url);
            var ticker = _setting.GetTickerFromUrl(url);
            T obj = ConvertResult(sr, ticker);
            return obj;
        }

        protected abstract T ConvertResult(string contentStr, string ticker="");
        protected abstract T ConvertResult(StreamReader streamReader, string ticker = "");

        public async Task<T> DownloadObjectTaskAsync(BaseSetting setting)
        {
            return await DownloadObjectTaskAsync(setting.GetUrlInternal());
        }

        public async Task<string> DownloadStringTaskAync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.Value.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var contentString = await response.Content.ReadAsStringAsync();

            return contentString;
        }

        public async Task<StreamReader> DownloadStreamTaskAync(string url)
        {
            var response = await _httpClient.Value.GetStreamAsync(url);
            var reader = new StreamReader(response);
            
            return reader;
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

        public int ParallelCapacity { get { return Environment.ProcessorCount; } }

        #region Batch Download

        public async Task<IEnumerable<FileInfo>> BatchDownloadFilesTaskAsync(IEnumerable<string> urls)
        {
            if (urls == null)
                throw new ArgumentNullException("urls is null");
            IEnumerable<FileInfo> output = null;
            try
            {
                output =await urls.ForEachAsync(ParallelCapacity, url => DownloadFileTaskAsync(url));
               
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return output;
        }

        public async Task<IEnumerable<T>> BatchDownloadObjectsTaskAsync(IEnumerable<string> urls)
        {            
            if (urls == null)
                throw new ArgumentNullException("urls is null");
            IEnumerable<T> output = null;
            try
            {
                output = await urls.ForEachAsync<string, T>(ParallelCapacity, url => DownloadObjectTaskAsync(url));
            }
            catch (Exception ex)
            {
                Debug.Write (ex.Message);
            }
            return output;
        }

        public async Task<IEnumerable<T>> BatchDownloadObjectsStreamTaskAsync(IEnumerable<string> urls)
        {

            if (urls == null)
                throw new ArgumentNullException("urls is null");
            IEnumerable<T> output = null;
            try
            {
                output = await urls.ForEachAsync<string, T>(ParallelCapacity, url => DownloadObjectStreamTaskAsync(url));
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return output;
            //if (urls == null)
            //    throw new ArgumentNullException("urls is null");
            //var list = new List<IEnumerable<T>>();
            //foreach (var url in urls)
            //{
            //    var result = await DownloadObjectStreamTaskAsync(url);
            //    list.Add(result);
            //}
            //IEnumerable<T> output = await urls.ForEachAsync<T>(url => DownloadObjectStreamTaskAsync(url));
            //return list;
        }
        #endregion

        #region IPostDownload
        
        public async Task<T> PostDownload(Uri url, Dictionary<string, string> data)
        {
            string contentStr = await PostDownloadInternal(url, data);
            T obj = ConvertResult(contentStr, "");
            return obj;
        }

        public async Task<FileInfo> PostDownloadFile(Uri url, Dictionary<string, string> data, string fileName)
        {
            var postData = new FormUrlEncodedContent(data);
            var response = await _httpClient.Value.PostAsync(url, postData);
            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsFileAsync(fileName, true);
            return new FileInfo(fileName);
        }

        private async Task<string> PostDownloadInternal (Uri url, Dictionary<string, string> data )
        {
            var postData = new FormUrlEncodedContent(data);
            var response = await _httpClient.Value.PostAsync(url, postData);
            response.EnsureSuccessStatusCode();
            string contentStr = await response.Content.ReadAsStringAsync();
            return contentStr;
        }
        #endregion
    }
}
