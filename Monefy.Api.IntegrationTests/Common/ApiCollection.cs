using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Monefy.Api.IntegrationTests.Common
{
    [CollectionDefinition("ApiCollection")]
    public class DBCollection : ICollectionFixture<ApiServerSetup>
    {
    }
}
