using Monefy.Api.Models.Expenses;
using Monefy.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monefy.Queries.Interfaces
{
    public interface IExpenseQueryProcessor
    {
        IQueryable<Expense> Get();

        Expense Get(int id);

        Task<Expense> Create(CreateExpenseModel createExpenseModel);

        Task<Expense> Update(UpdateExpenseModel updateExpenseModel);

        Task<Expense> Delete(int id);

    }
}
