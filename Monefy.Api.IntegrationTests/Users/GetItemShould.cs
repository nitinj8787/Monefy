using FluentAssertions;
using Monefy.Api.IntegrationTests.Common;
using Monefy.Api.IntegrationTests.Helpers;
using Monefy.Api.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Monefy.Api.IntegrationTests.Users
{
    [Collection("ApiCollection")]
    public class GetItemShould
    {
        private ApiServerSetup apiServer;
        private HttpClient httpClient;
                
        public GetItemShould()
        {
            apiServer = new ApiServerSetup();
            httpClient = apiServer.Client;
        }

        [Fact]
        public async Task ReturnItemById()
        {
            var id = 1;
            var result = await httpClient.GetAsync($"api/users/{id}");

            // test case
            result.EnsureSuccessStatusCode();
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((new HttpResponseMessage {  StatusCode = System.Net.HttpStatusCode.OK }).StatusCode);

            var resposne = result.Content.ReadAsStringAsync().Result;
            resposne.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ShouldReturn404ForIncorrectUriRequest()
        {
            var id = -1;
            var result = await httpClient.GetAsync(new Uri($"api/users/{id}", UriKind.Relative));
                                            
            result.StatusCode.Should().Be((new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NotFound }).StatusCode);
        }

        public async Task<UserModel> GetItemById(HttpClient client, int id)
        {
            var result = await client.GetAsync($"api/users/{id}");

            return JsonConvert.DeserializeObject<UserModel>(result.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task ShouldReturnForbiddenExceptionForUserWhoIsNotAdministratorOrManager()
        {
            var username = "user";
            var password = "user";

             var client = apiServer.GetAuthenticatedClient(username, password);

            var id = 5;
            var result = await client.GetAsync(new Uri($"api/users/{id}", UriKind.Relative));

            var response = JsonConvert.DeserializeObject<UserWithTokenModel>(result.Content.ReadAsStringAsync().Result);

            result.StatusCode.Should().Be((new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Forbidden }).StatusCode);
        }

    }
}
