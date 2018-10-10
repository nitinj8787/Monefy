using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Monefy.Data.Model;
using System.Threading.Tasks;
using Monefy.Api.Models.User;

namespace Monefy.Queries.Interfaces
{
    public interface IUserQueryProcessor
    {
        IQueryable<User> Get();

        User Get(int id);

        Task<User> Create(CreateUserModel createUserModel);

        Task<User> Update(int id, UdpateUserModel updateUserModel);

        Task ChangePassword(int id, ChangeUserPasswordModel changeUserPasswordModel);

        Task Delete(int id);

    }
}
