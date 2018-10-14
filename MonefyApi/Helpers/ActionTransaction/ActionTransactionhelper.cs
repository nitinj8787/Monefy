using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Monefy.Data.Access.Interfaces;

namespace MonefyApi.Helpers.ActionTransaction
{
    public class ActionTransactionHelper : IActionTransactionHelper
    {
        private IUnitOfWork _uow;
        private ITransaction _transaction;

        private readonly ILogger _logger;

        private bool TransactionHandled { get; set; }
        public bool SessionClosed { get; set; }

        public ActionTransactionHelper(IUnitOfWork unitOfWork , ILogger logger)
        {
            _uow = unitOfWork;
            _logger = logger;
        }

        public void BeginTransaction()
        {
            _transaction = _uow.BeginTransaction();
        }

        public void EndTransaction(ActionExecutedContext filterContext)
        {
            if (_transaction == null)
                throw new NotSupportedException();

            if(filterContext.Exception  == null)
            {
                _uow.Commit();
                _transaction.Commit();
            }
            else
            {
                try
                {
                    _transaction.Rollback();
                }
                catch (Exception ex)
                {
                    throw new AggregateException(filterContext.Exception, ex);
                }
            }

            TransactionHandled = true;
        }

        public void CloseSession()
        {
            if(_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
            if(_uow != null)
            {
                _uow.Dispose();
                _uow = null;
            }

            SessionClosed = true;
        }
    }
}
