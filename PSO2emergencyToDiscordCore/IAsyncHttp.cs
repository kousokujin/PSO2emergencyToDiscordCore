//using System;
//using System.Collections.Generic;
//using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace PSO2emergencyToDiscordCore
{
    interface IAsyncHttp
    {
        void setHTTPClient(HttpClient hc);
        string getUrl();
    }

    interface IAsyncPOST : IAsyncHttp
    {
        //Task<HttpResponseMessage> AsyncHttpPOST(StringContent content);
        Task<string> AsyncHttpPOST(StringContent content);
    }

    interface IAsyncGET : IAsyncHttp
    {
        Task<string> AsyncHttpGET();
    }
}
