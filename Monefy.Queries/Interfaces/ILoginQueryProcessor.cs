using Monefy.Api.Models.Login;
using Monefy.Api.Models.User;
using Monefy.Data.Model;
using Monefy.Queries.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monefy.Queries.Interfaces
{
    public interface ILoginQueryProcessor
    {
        UserWithToken Authenticate(string username, string password);

        Task<User> Register(RegisterModel model);

        Task ChangePassword(ChangeUserPasswordModel requestModel);
    }
}
