using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monefy.Api.Models.User;
using Monefy.Data.Access.Constants;
using Monefy.Data.Model;
using Monefy.Queries.Interfaces;
using MonefyApi.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MonefyApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.AdministratorOrManager)]
    public class UsersController : Controller
    {
        private readonly IUserQueryProcessor _userQueryProcessor;
        private readonly IAutoMapper _autoMapper;

     
        public UsersController(IUserQueryProcessor userQueryProcessor, IAutoMapper autoMapper )
        {
            _userQueryProcessor = userQueryProcessor;
            _autoMapper = autoMapper;
         }

        [HttpGet]
        public IQueryable<UserModel> Get()
        {
            var result = _userQueryProcessor.Get();

            var model = _autoMapper.Map<User, UserModel>(result);

            return model;
        }

        [HttpGet("{id}")]
        public UserModel Get(int id)
        {
            
            var result = _userQueryProcessor.Get(id);

            var model = _autoMapper.Map<UserModel>(result);

            return model;
        }

        [HttpDelete("{Id}")]
        public async Task Delete(int id)
        {
            await _userQueryProcessor.Delete(id);

        }
    }
}
