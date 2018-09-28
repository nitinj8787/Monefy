using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monefy.Data.Access.Interfaces
{
    public interface IMap
    {
        void Visit(ModelBuilder builder);
    }
}
