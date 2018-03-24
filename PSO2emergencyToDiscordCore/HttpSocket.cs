using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace PSO2emergencyToDiscordCore
{
    class HttpSocket : IAsyncHttp
    {
        public string url;
        protected Encoding encode;
        protected HttpClient hc;

        public HttpSocket(string url,Encoding enc,HttpClient cl)
        {
            this.url = url;
            this.encode = enc;
            setHTTPClient(cl);
        }

        public void setHTTPClient(HttpClient hc)
        {
            this.hc = hc;
        }

        public string getUrl()
        {
            return url;
        }
    }
}
