using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonefyApi.Helpers.ActionTransaction
{
    public interface IActionTransactionHelper
    {
        void BeginTransaction();

        void EndTransaction(ActionExecutedContext filterContext);

        void CloseSession();
    }
}
