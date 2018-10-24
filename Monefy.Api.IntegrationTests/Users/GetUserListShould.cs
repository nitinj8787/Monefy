using Monefy.Api.IntegrationTests.Common;
using Monefy.Api.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Monefy.Api.IntegrationTests.Users
{
    [Collection("ApiCollection")]
    public class GetUserListShould
    {
        private ApiServerSetup _apiServer;
        private HttpClient _httpClient;

        private Random _random;

        public GetUserListShould(ApiServerSetup apiServerSetup)
        {
            _apiServer = apiServerSetup;
            _httpClient = _apiServer.Client;
        }

        [Fact]
        public async Task ReturnAnyList()
        {
            var expectedResposne = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };

            var result = await _httpClient.GetAsync("api/Users");

            // test case
            result.EnsureSuccessStatusCode();
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(expectedResposne.StatusCode);

            var resposne = result.Content.ReadAsStringAsync().Result;
            resposne.Should().NotBeEmpty();
        }

    }
}
