using Monefy.Api.Common.Exceptions;
using Monefy.Api.Models.Login;
using Monefy.Api.Models.User;
using Monefy.Data.Access.Interfaces;
using Monefy.Data.Model;
using Monefy.Queries.Interfaces;
using Monefy.Queries.Models;
using Monefy.Security.Auth;
using Monify.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monefy.Api.Common.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Monefy.Queries.Queries
{
    public class LoginQueryProcessor : ILoginQueryProcessor
    {
        private readonly IUnitOfWork _uow;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IUserQueryProcessor _userQueryProcessor;
        private readonly ISecurityContext _securityContext;

        private Random _random;

        public LoginQueryProcessor(IUnitOfWork unitOfWork, ITokenBuilder tokenBuilder, IUserQueryProcessor userQueryProcessor, ISecurityContext securityContext)
        {
            _random = new Random();
            _uow = unitOfWork;
            _tokenBuilder = tokenBuilder;
            _userQueryProcessor = userQueryProcessor;
            _securityContext = securityContext;
        }

        public UserWithToken Authenticate(string username, string password)
        {
            var usr = _uow.Query<User>()
                        .Where(x => x.Username == username && !x.IsDeleted)
                        .Include(x => x.Roles)
                        .ThenInclude(x => x.Role)
                        .FirstOrDefault();

           
            if(usr == null)
            {
                throw new BadRequestException("incorrect username or password");
            }

            if(string.IsNullOrWhiteSpace(password) || !usr.Password.VerifyWithBCrypt(password))
            {
                throw new BadRequestException("incorrect username or password");
            }

            var expiresIn = DateTime.Now + TokenAuthOption.ExpiresSpan;

            var token = _tokenBuilder.BuildToken(usr.Username, usr.Roles.Select(x => x.Role.Name).ToArray(), expiresIn);

            return new UserWithToken
            {
                ExpiresAt = expiresIn,
                Token = token,
                User = usr
            };
        }

      
        public async Task ChangePassword(ChangeUserPasswordModel requestModel)
        {
            await _userQueryProcessor.ChangePassword(_securityContext.User.Id, requestModel);
        }

        public async Task<User> Register(RegisterModel model)
        {
            var creteUserModel = new CreateUserModel
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password
            };

            var user = await _userQueryProcessor.Create(creteUserModel);

            return user;
        }
    }
}
