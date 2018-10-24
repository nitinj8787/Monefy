using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Monefy.Api.IntegrationTests.Helpers;
using Monefy.Api.Models.Login;
using Monefy.Api.Models.User;
using MonefyApi;
using Newtonsoft.Json;
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
        public const string password = "admin";

        public IConfigurationRoot _config;

        public HttpClient Client { get; set; }
        public TestServer Server { get; set; }

        public ApiServerSetup()
        {
            _config = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json")
                             .Build();

            Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = GetAuthenticatedClient(userName, password);

        }

        public HttpClient GetAuthenticatedClient(string username, string password)
        {
            var client = Server.CreateClient();

            var response = client.PostAsync("/api/Login/Authenticate",
                new JsonContent(new LoginModel { UserName = username, Password = password })).Result;

            response.EnsureSuccessStatusCode();

            var data = JsonConvert.DeserializeObject<UserWithTokenModel>(response.Content.ReadAsStringAsync().Result);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + data.Token);

            return client;
        }

        public void Dispose()
        {
            if(Client != null)
            {
                Client.Dispose();
                Client = null;
            }

            if(Server != null)
            {
                Server.Dispose();
                Server = null;
            }
        }
    }
}
