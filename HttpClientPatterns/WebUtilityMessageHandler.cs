using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace HttpClientPatterns
{
    public class WebUtilityMessageHandler : HttpClientHandler
    {
        private readonly ICredentialProvider credentialProvider;

        public WebUtilityMessageHandler()
        {
        }

        public WebUtilityMessageHandler(ICredentialProvider credentialProvider)
        {
            this.credentialProvider = credentialProvider;
        }


        // This message handler  decorates all outgoing requests with headers common to all
        // outgoing messages sent by IWebUtility. It demonstrates making async calls to
        // acquire application specific information from an injected provider that is needed 
        // to decorate messages.
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var credential = await credentialProvider.GetFakeCredentialAsync();

            request.Headers.Add("x-fake-credential", credential);
            request.Headers.Add("x-webutilty-example", DateTime.Now.ToLongDateString());

            // because this function uses async to get the fake credential it must also return await on the 
            // base class call so that an HttpResposneMessage is returned.
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
