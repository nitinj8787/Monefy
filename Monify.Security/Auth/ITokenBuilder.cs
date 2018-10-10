using System;
using System.Collections.Generic;
using System.Text;

namespace Monefy.Security.Auth
{
    public interface ITokenBuilder
    {
        string BuildToken(string name, string[] roles, DateTime tokenExpirationDate);
    }
}
