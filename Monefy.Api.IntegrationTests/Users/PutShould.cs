using FluentAssertions;
using Monefy.Api.IntegrationTests.Common;
using Monefy.Api.IntegrationTests.Helpers;
using Monefy.Api.IntegrationTests.Login;
using Monefy.Api.Models.User;
using Monefy.Data.Access.Constants;
using Monefy.Data.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Monefy.Api.IntegrationTests.Users
{
    [Collection("ApiCollection")]
    public class PutShould
    {
        private readonly ApiServerSetup apiServer;
        private readonly HttpClientWrapper client;

        public PutShould(ApiServerSetup apiServerSetup)
        {
            apiServer = apiServerSetup;
            client = new HttpClientWrapper(apiServer.Client);
        }

        [Fact]
        public async Task UpdateExistingItem()
        {
            var newItem = new PostShould(apiServer).ShouldRegisterNewUser().Result;

            var item = new UserModel
            {
                FirstName = "updated_" + newItem.FirstName,
                LastName = "updated_" + newItem.LastName,
                Username = "updated_" +  newItem.Username,
                Roles = new[] { Roles.Manager }
            };

            var result = await client.PutAsync<UserModel>($"api/users/{item.Id}", item);

            // fetch the updated value 
            var updatedResult = await new GetItemShould().GetItemById(client.Client, item.Id);

            updatedResult.FirstName.Should().Be(item.FirstName);
            updatedResult.LastName.Should().Be(item.LastName);
            updatedResult.Username.Should().Be(item.Username);

            updatedResult.Roles.Length.Should().BeGreaterThan(0);

        }

    }
}
