using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace PSO2emergencyToDiscordCore
{
    interface IAsyncHttp
    {
        void setHTTPClient(HttpClient hc);
        Task<HttpResponseMessage> AsyncHttpPOST(StringContent content);
        string getUrl();
    }
}
