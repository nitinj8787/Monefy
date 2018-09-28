using Microsoft.EntityFrameworkCore.Storage;
using Monefy.Data.Access.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monefy.Data.Access.DAL
{
    public class DbTransaction : ITransaction
    {
        private readonly IDbContextTransaction _efTransaction;

        public DbTransaction(IDbContextTransaction dbContextTransaction)
        {
            _efTransaction = dbContextTransaction;
        }
        public void Commit()
        {
            _efTransaction.Commit();
        }

        public void Dispose()
        {
            _efTransaction.Dispose();
        }

        public void Rollback()
        {
            _efTransaction.Rollback();
        }
    }
}
