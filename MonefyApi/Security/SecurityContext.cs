using Monefy.Data.Access.Interfaces;
using Monefy.Data.Model;
using Monify.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Monefy.Api.Common;
using Monefy.Data.Access.Constants;

namespace MonefyApi.Security
{
    public class SecurityContext : ISecurityContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        private User _user;

        public SecurityContext(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public User User
        {
            get
            {
                if (_user != null) return _user;

                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    throw new UnauthorizedAccessException();

                var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
                _user = _unitOfWork.Query<User>().Where(u => u.Username == userName)
                                                    .Include(u => u.Roles)
                                                    .ThenInclude(u => u.Role)
                                                    .FirstOrDefault();

                if (_user == null)
                    throw new NotFoundException("User not found");

                return _user;
            }
        }

        public bool IsAdministrator
        {
            get
            {
                return User.Roles.Any(x => x.Role.Name == Roles.Administrator);
            }
        }

    }
}
