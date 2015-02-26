using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace NB.Core.Web.DownloadSettings
{
    public abstract partial class BaseSetting //: ICloneable
    {
        public abstract string GetUrl();
        public abstract string GetUrl(string ticker);

        public abstract string GetTickerFromUrl(string url);
        public abstract string GetFileName(string ticker);
        public abstract string GetFileName();

       // public abstract object Clone();
        public string Ticker {get; set;}

        private List<KeyValuePair<string, string>> mAdditionalHeaders = new List<KeyValuePair<string, string>>();

        protected virtual List<KeyValuePair<string, string>> AdditionalHeaders
        {
            get
            {
                return mAdditionalHeaders;
            }
        }
        protected virtual HttpMethod Method { get { return HttpMethod.Get; } }
        protected virtual CookieContainer Cookies { get { return null; } }
        protected virtual string ContentType { get { return string.Empty; } }
        protected virtual string PostData { get { return string.Empty; } }
        protected virtual bool DownloadResponseStream { get { return true; } }
        
        /*base client to interact with internal methods which will be overriden by sub setting class*/
        internal string GetUrlInternal() { return this.GetUrl(); }
        internal List<KeyValuePair<string, string>> GetAdditionalHeadersInternal { get { return mAdditionalHeaders; } }
        internal HttpMethod MethodInternal { get { return this.Method; } }
        internal CookieContainer CookiesInternal { get { return this.Cookies; } }
        internal string ContentTypeInternal { get { return this.ContentType; } }
        internal string PostDataInternal { get { return this.PostData; } }
        internal bool DownloadResponseStreamInternal { get { return this.DownloadResponseStream; } }

        //For Desktop
        protected virtual bool KeepAlive { get { return true; } }
        internal bool KeepAliveInternal { get { return this.KeepAlive; } }
    }    
}
//http://query.yahooapis.com/v1/public/yql?q=show+tables