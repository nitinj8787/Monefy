using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Monefy.Data.Access.Interfaces;
using Monefy.Data.Model;

namespace Monefy.Data.Access.Maps.Main
{
    public class UserMap : IMap
    {
        public void Visit(ModelBuilder builder) => builder.Entity<User>().ToTable("Users").HasKey(x => x.Id);

    }
}
