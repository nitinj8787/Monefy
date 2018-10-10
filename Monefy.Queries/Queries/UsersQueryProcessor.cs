using Microsoft.EntityFrameworkCore;
using Monefy.Api.Common;
using Monefy.Api.Common.Exceptions;
using Monefy.Api.Common.Extentions;
using Monefy.Api.Models.User;
using Monefy.Data.Access.Interfaces;
using Monefy.Data.Model;
using Monefy.Queries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monefy.Queries.Queries
{
    public class UsersQueryProcessor : IUserQueryProcessor
    {
        private readonly IUnitOfWork _uow;

        public UsersQueryProcessor(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<User> Create(CreateUserModel createUserModel)
        {
            //check if firstname is unique
            var suppliedName = GetUserQuery().Any(x => x.Username.Trim() == createUserModel.UserName.Trim());

            if (suppliedName)
                throw new BadRequestException("User aleady exist");

            var user = new User
            {
                FirstName = createUserModel.FirstName.Trim(),
                LastName = createUserModel.LastName.Trim(),
                Password = createUserModel.Password.Trim().WithBCrypt(),
                //ConfirmPassword = createUserModel.ConfirmPassword.VerifyWithBCrypt(createUserModel.ConfirmPassword)? true:false,
                Username = createUserModel.UserName.Trim()
            };

            AddUserRoles(user, createUserModel.Roles);

            _uow.Add(user);
            await _uow.CommitAsync();

            return user;
        }

        private void AddUserRoles(User user, string[] roles)
        {
            // if role exist clear all role 
            user.Roles.Clear();

            foreach (var r in roles)
            {
                var role = _uow.Query<Role>().Where(x => x.Name == r).FirstOrDefault();

                if (role == null)
                    throw new NotFoundException("Role does not exist");

                user.Roles.Add(new UserRole { Role = role, User = user });
            }
        }

        public async Task Delete(int id)
        {
            var isUserExist = GetUserQuery().Any(x => x.Id == id);

            if (!isUserExist)
                throw new NotFoundException("User does not exist");

            var user = GetUserQuery().Where(x => x.Id == id).FirstOrDefault();

            if (user.IsDeleted)
                return;
            else
            {
                user.IsDeleted = true;
                await _uow.CommitAsync();
            }
        }

        public IQueryable<User> Get()
        {
            return GetUserQuery();
        }

        private IQueryable<User> GetUserQuery()
        {
            var userList = _uow.Query<User>()
                .Where(x => !x.IsDeleted)
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role);

            return userList;
        }

        public User Get(int id)
        {
            var usr = GetUserQuery().Where(x => x.Id == id).FirstOrDefault();

            if (usr == null || usr.IsDeleted)
            {
                throw new NotFoundException("User not found");
            }

            return usr;
        }

        public async Task<User> Update(int id, UdpateUserModel updateUserModel)
        {
            var isUsrExist = GetUserQuery().Any(x => x.Id == id);

            if (!isUsrExist)
                throw new NotFoundException("USer does not exist");

            var usrToUpdate = GetUserQuery().Where(x => x.Id == id).FirstOrDefault();

            //usrToUpdate.Roles.Clear();

            //var isRoleExist = usrToUpdate.Roles.Any(x => x.Role == usrToUpdate.Roles);
            //if (!isRoleExist)
            //{
            //    foreach (var r in updateUserModel.Roles)
            //    {
            //        var role = _uow.Query<UserRole>().Where(x => x.Role.Name == r).FirstOrDefault();
            //        usrToUpdate.Roles.Add(role);
            //    }
            //}

            // add role
            AddUserRoles(usrToUpdate, updateUserModel.Roles);

            // update details
            usrToUpdate.Username = updateUserModel.Username;
            usrToUpdate.LastName = updateUserModel.LastName;
            usrToUpdate.FirstName = updateUserModel.FirstName;
            
            _uow.Update<User>(usrToUpdate);

            await _uow.CommitAsync();

            return usrToUpdate;

        }

        public async Task ChangePassword(int id, ChangeUserPasswordModel changeUserPasswordModel)
        {
            var usr = Get(id);

            usr.Password = changeUserPasswordModel.Password.WithBCrypt();

            await _uow.CommitAsync();
        }
    }
}
