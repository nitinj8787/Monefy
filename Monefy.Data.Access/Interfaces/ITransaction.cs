using System;
using System.Collections.Generic;
using System.Text;

namespace Monefy.Data.Access.Interfaces
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
