using Monefy.Data.Model;
using System;

namespace Monify.Security
{
    public interface ISecurityContext
    {
        User User { get; }

        bool IsAdministrator { get; }
    }
}
