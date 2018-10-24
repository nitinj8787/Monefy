using FluentAssertions;
using Monefy.Api.IntegrationTests.Common;
using Monefy.Api.IntegrationTests.Helpers;
using Monefy.Api.Models.Login;
using Monefy.Api.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Monefy.Api.IntegrationTests.Login
{
    [Collection("ApiCollection")]
    public class PostShould
    {
        private readonly ApiServerSetup _apiServer;
        //private readonly HttpClientWrapper _clientWrapper;

        private readonly HttpClientWrapper _client;

        private readonly HttpClient _httpClient;

        private readonly Random _random;

        public PostShould(ApiServerSetup apiServerSetup)
        {
            _apiServer = apiServerSetup;
            _httpClient = _apiServer.Client;
            _client = new HttpClientWrapper(_apiServer.Client);
            _random = new Random();
        }

        [Fact]
        public async Task AuthenticateAdmin()
        {
            var username = "admin";
            var password = "admin";

            var result = await Authenticate(username, password);

            result.User.Username.Should().Be(username);
        }

        public async Task<UserWithTokenModel> Authenticate(string username, string password)
        {
            var resposne = await _httpClient.PostAsync("/api/Login/Authenticate", new JsonContent
                (new LoginModel
                {
                    UserName = username,
                    Password = password
                }
                ));

            var responseModel = JsonConvert.DeserializeObject<UserWithTokenModel>(resposne.Content.ReadAsStringAsync().Result);

            return responseModel;
        }

        [Fact]
        public async Task<UserModel> ShouldRegisterNewUser()
        {
            var username = "user_" + _random.Next();
            var password = _random.Next().ToString();
            var firstname = _random.Next().ToString();
            var lastname = _random.Next().ToString();

            var result = await _client.PostAsync<UserModel>("api/Login/Register", new RegisterModel
            {
                FirstName = firstname,
                LastName = lastname,
                UserName = username,
                Password = password
            });

            result.Username.Should().Be(username);
            result.FirstName.Should().Be(firstname);
            result.LastName.Should().Be(lastname);
            result.Id.Should().BePositive();
            result.Roles.Should().BeEmpty();


            return result;
        }

        [Fact]
        public async Task ChangeUserPassword()
        {
            var requestItem = new RegisterModel
            {
                UserName = "TU_" + _random.Next(),
                Password = _random.Next().ToString(),
                LastName = _random.Next().ToString(),
                FirstName = _random.Next().ToString()
            };

            await _client.PostAsync<UserModel>("api/Login/Register", requestItem);

            var newClient = new HttpClientWrapper(_apiServer.GetAuthenticatedClient(requestItem.UserName, requestItem.Password));

            var newPassword = _random.Next().ToString();
            await newClient.PostAsync($"api/Login/ChangePassword", new ChangeUserPasswordModel
            {
                Password = newPassword,
                ConfirmPassword = newPassword
               
            });

            await Authenticate(requestItem.UserName, newPassword);
        }
    }
}
