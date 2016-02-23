using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Net.Http;

namespace HttpClientPatterns
{
    class Program
    {
        private static UnityContainer dependencyContainer;

        // The Main function kicks off the demonstration of registering dependencies for HttpClient dependency
        // injection and then calls a function on WebUtility to do some work. The interesting things
        // happen in WebUtility and it's HttpMessageHandler derived message handler called WebUtilityMessageHandler.
        // Please refer to the comments in those two classes for more details and also the unit test for
        // how to Mock HttpClient
        static void Main(string[] args)
        {
            InitializeDependencies();
            var webUtility = dependencyContainer.Resolve<IWebUtility>();
            var resultTask = webUtility.GetResourceAsync("http://api.openweathermap.org/data/2.5/weather?q=Redmond,usa&appid=44db6a862fba0b067b1930da0d769e98");
            resultTask.Wait();
            Debug.WriteLine(resultTask.Result);
        }

        private static void InitializeDependencies()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IWebUtility, WebUtility>();
            container.RegisterType<ICredentialProvider, CredentialProvider>();
            container.RegisterType<WebUtilityMessageHandler>();
            dependencyContainer = container;
        }
    }
}
