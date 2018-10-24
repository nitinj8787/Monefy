using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Monefy.Api.IntegrationTests.Helpers
{
    public class JsonContent : StringContent
    {
        public JsonContent(object content) : base(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
        {
        }
    }
}
