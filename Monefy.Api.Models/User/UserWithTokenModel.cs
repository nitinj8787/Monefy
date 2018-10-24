using System;
using System.Collections.Generic;
using System.Text;

namespace Monefy.Api.Models.User
{
    public class UserWithTokenModel
    {
        public string Token { get; set; }

        public UserModel User { get; set; }

        public DateTime ExpiressAt { get; set; }
    }
}
