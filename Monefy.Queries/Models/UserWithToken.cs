using Monefy.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monefy.Queries.Models
{
    public class UserWithToken
    {
        public string Token { get; set; }

        public User User { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
