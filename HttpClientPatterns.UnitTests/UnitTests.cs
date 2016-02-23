using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Globalization;
using System.Text;

namespace HttpClientPatterns.UnitTests
{
    [TestClass]
    public class UnitTests
    {
        // This test demonstrates how a service class that has a dependency on HttpClient can be mocked.
        // The HttpClient class is already designed to be mocked using the HttpMessageHandler derived type.
        // THe only trick to get it to work with the Moq library is that you have to use Moq.Protected to 
        // mock the HttpMessageHandler's protected SendAsync function.
        [TestMethod]
        public async Task TestUtilty()
        {
            var fakeResponseContent = "Here is a fake response body";
            var messageHandlerMock = MockWebUtilityMessageHandler(fakeResponseContent);
            var webUtility = new WebUtility(messageHandlerMock);
            var result = await webUtility.GetResourceAsync("http://someresource");
            Assert.AreEqual(fakeResponseContent, result);
        }

        // This is a helper function that shows how to mock the protected HttpMessageHandler.SendAsync function
        // using the Moq library.
        public WebUtilityMessageHandler MockWebUtilityMessageHandler(string fakeResponseContent)
        {
            var messageHandlerMock = new Mock<WebUtilityMessageHandler>();
            CancellationToken ct = CancellationToken.None;


            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            responseMessage.Content = new StringContent(fakeResponseContent, Encoding.UTF8, "text/json");

            // Mock's protected SendAsync method returns the fake response message created above.
            messageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(responseMessage));

            return messageHandlerMock.Object;
        }
    }
}
