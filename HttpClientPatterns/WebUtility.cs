using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace HttpClientPatterns
{
    public class WebUtility : IWebUtility
    {
        HttpClient client;

        // All WebUtility outgoing HTTP messages get decorated with headers
        // common to WebUtility messages using the WebUtilityMessageHandler,
        // which is an HttpMessageHandler derived type that decorates messages
        // before they are sent with WebUtility specific headers.
        // Also, by injecting an HttpMessageHandler dervied type and using that
        // to construct an HttpClient, the HttpClient's SendAsync behavior can
        // be Mocked for unit testing using design patterns included with HttpClient
        public WebUtility(WebUtilityMessageHandler handler)
        {
            this.client = new HttpClient(handler);
        }


        // This function uses the HttpClient.GetStringAsync function which is
        // a higher level wrapper around HttpClient.SendAsync. By injecting a
        // custom HttpMessageHandler into this class, the HttpClient.GetStringAsync
        // behavior can be Mocked because HttpClient.GetStringAsync calls 
        // HttpClient.SendAsync and the HttpClient.SendAsync behavior is in turn
        // controlled by the HttpMessageHandler derived type that was injected
        // into this class
        public Task<string> GetResourceAsync(string url)
        {
            return client.GetStringAsync(url);
        }
    }
}
