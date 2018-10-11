using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;


namespace Monefy.Security.Auth
{
    public class TokenBuilder : ITokenBuilder
    {
        public string BuildToken(string name, string[] roles, DateTime tokenExpirationDate)
        {
            var handler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>();

            foreach (var userRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(name, "Bearer"), claims);

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.signingCredentials,
                Subject = identity,
                Expires = tokenExpirationDate
            });

            return handler.WriteToken(securityToken);
        }
    }
}
