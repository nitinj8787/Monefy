using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Monefy.Security.Auth
{
    public class TokenAuthOption
    {
        public static string Audience { get; } = "monefy_audience";
        public static string Issuer { get; } = "money_issue";

        public static RsaSecurityKey key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey());

        public static SigningCredentials signingCredentials { get; } = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(30);

        public static string TokenType { get; } = "Bearer";

    }
}
