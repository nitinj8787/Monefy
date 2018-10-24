using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monefy.Api.Models.Login;
using Monefy.Api.Models.User;
using Monefy.Data.Model;
using Monefy.Queries.Interfaces;
using Monefy.Queries.Models;
using MonefyApi.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonefyApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[Controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginQueryProcessor _loginQueryProcessor;
        private readonly IAutoMapper _autoMapper;


        public LoginController(ILoginQueryProcessor loginQueryProcessor, IAutoMapper autoMapper)
        {
            _loginQueryProcessor = loginQueryProcessor;
            _autoMapper = autoMapper;
        }

        [HttpPost("Authenticate")]
        public UserWithTokenModel Authenticate([FromBody] LoginModel loginModel)
        {
            var result = _loginQueryProcessor.Authenticate(loginModel.UserName, loginModel.Password);

            var resultModel = _autoMapper.Map<UserWithTokenModel>(result);

            return resultModel;

        }

        [HttpPost("Register")]
        public async Task<UserModel> RegisterNewUser([FromBody] RegisterModel registerModel)
        {
            var result = await _loginQueryProcessor.Register(registerModel);

            var resultModel = _autoMapper.Map<UserModel>(result);

            return resultModel;

            
        }

        [HttpPost("ChangePassword")]
        public async Task ChangePassword([FromBody] ChangeUserPasswordModel changeUserPasswordModel)
        {
             await _loginQueryProcessor.ChangePassword(changeUserPasswordModel);
        }

    }
}
