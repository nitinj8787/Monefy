using Monefy.Api.Models.Expenses;
using Monefy.Data.Model;
using Monefy.Queries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monefy.Queries.Queries
{
    public class ExpenseQueryProcessor : IExpenseQueryProcessor
    {
        public Task<Expense> Create(CreateExpenseModel createExpenseModel)
        {
            throw new NotImplementedException();
        }

        public Task<Expense> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Expense> Get()
        {
            throw new NotImplementedException();
        }

        public Expense Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Expense> Update(UpdateExpenseModel updateExpenseModel)
        {
            throw new NotImplementedException();
        }
    }
}
