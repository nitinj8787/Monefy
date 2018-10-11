using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Monefy.Api.IntegrationTests.Common
{
    public class ApiServerSetup : IDisposable
    {
        public const string userName = "admin";
        public const string password = "admin1";

        public IConfigurationRoot _config;

        public HttpClient Client { get; set; }
        public TestServer Server { get; set; }

        public ApiServerSetup()
        {
            _config = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("apiSettings")
                             .Build();

            //Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
           // Client = GetAuthenticatedClient(Username, Password);

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
