using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;

namespace wget
{
    class YahooCsvDownloader
    {
        static HttpClient httpClient = new HttpClient();
        private delegate Task<string> DownloadDelegate(string url);

        public static void StartDownload(YahooDownloadParam param)
        {
            Parallel.ForEach(param.GetUrls(), new ParallelOptions { MaxDegreeOfParallelism = ParallelCapacity }, DownloadFile);
        }


        public static void DownloadFile(string url)
        {
            var file = GetTickerFromUrl(url);
            var response = HttpWebRequest.Create(url).GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                using (var sw = new StreamWriter(FileName(file), false))
                {
                    sw.Write(sr.ReadToEnd());
                }
            }
        }

        private static string GetTickerFromUrl(string url)
        {
            var match = Regex.Match(url, YahooDownloadParam.MatchStr);
            var file = match.Groups["ticker"].Value;
            return file;
        }

        public static async Task BatchDownloadFiles(IEnumerable<string> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");
            await enumerable.ForEachAsync(ParallelCapacity, s => DownloadFileAsync (s));
        }

        public static async Task<string> DownloadFileAsync(string url)
        {           
            Uri result;
            if (!Uri.TryCreate(url, UriKind.Absolute, out result))
            {
                Debug.WriteLine(string.Format("Couldn't create URI from specified address: {0}", url));
                return null;
            }
            try
            {
                string fileName = await DownloadFileTaskAsyncWithHttpClient(url);                    
                Debug.WriteLine(string.Format("Downloaded file saved to: {0} ({1})", fileName, url));
                return fileName;
                
            }
            catch (WebException webException)
            {
                Debug.WriteLine(string.Format("Couldn't download file from specified address: {0}", webException.Message));
                return null;
            }
        }

        public static async Task<string> DownloadFileTaskAsyncWithWebClient(string url)
        {
            var ticker = GetTickerFromUrl(url);
            string fileName = FileName(ticker);
            using (var client = new WebClient())
            {                
                await client.DownloadFileTaskAsync(url, fileName);                
            }
            return fileName;
        }

        public static async Task<string> DownloadFileTaskAsyncWithHttpClient(string url)
        {
            var ticker = GetTickerFromUrl(url);
            string fileName = FileName(ticker);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync();
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await contentStream.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public static async Task<string> DownloadStringTaskAyncWithHttpClient(string url)
        {        

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var contentString = await (await httpClient.SendAsync(request)).Content.ReadAsStringAsync();

            return contentString;
        }

        public static async Task<string> DownloadStringTaskAsyncWithWebClient(string url)
        {

            using (var client = new WebClient())
            {
                return await client.DownloadStringTaskAsync(url);
            }
        }

        public static string FileName(string symbol)
        {
            var path = string.Format(@"{0}\{1}-{2}.csv", Directory.GetCurrentDirectory(), symbol, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            return path;
        }

        public static int ParallelCapacity {get {return Environment.ProcessorCount*2;}}
    }
}
