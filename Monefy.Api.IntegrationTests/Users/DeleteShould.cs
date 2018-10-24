using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Monefy.Api.IntegrationTests.Common;
using Monefy.Api.IntegrationTests.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Monefy.Api.IntegrationTests.Users
{
    [Collection("ApiCollection")]
    public class DeleteShould
    {
        private readonly ApiServerSetup _apiServer;
        private readonly HttpClient _client;

        public DeleteShould(ApiServerSetup serverSetup)
        {
            _apiServer = serverSetup;
            _client = _apiServer.Client;
        }

        [Fact]
        public async Task DeleteExistingUser()
        {
            var item = await new Login.PostShould(_apiServer).ShouldRegisterNewUser();
            var userId = item.Id;

            var resposne = await _client.DeleteAsync($"api/Users/{userId}");
            resposne.EnsureSuccessStatusCode();
        }
    }
}
