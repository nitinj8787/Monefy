﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Monefy.Api.IntegrationTests.Helpers
{
    public class HttpClientWrapper
    {
        private readonly HttpClient _client;

        public HttpClientWrapper(HttpClient client)
        {
            _client = client;
        }

        public HttpClient Client => _client;

        public async Task<T> PostAsync<T>(string url, object body)
        {
            var response = await _client.PostAsync(url, new JsonContent(body));

            response.EnsureSuccessStatusCode();

            var respnoseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(respnoseText);
            return data;
        }

        public async Task PostAsync(string url, object body)
        {
            var response = await _client.PostAsync(url, new JsonContent(body));

            response.EnsureSuccessStatusCode();
        }

        public async Task<T> PutAsync<T>(string url, object body)
        {
            var response = await _client.PutAsync(url, new JsonContent(body));

            response.EnsureSuccessStatusCode();

            var respnoseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(respnoseText);
            return data;
        }
    }
}