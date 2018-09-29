using Monefy.Api.Models.Expenses;
using Monefy.Data.Access.DAL;
using Monefy.Data.Access.Interfaces;
using Monefy.Data.Model;
using Monefy.Queries.Interfaces;
using Monify.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monefy.Queries.Queries
{
    public class ExpenseQueryProcessor : IExpenseQueryProcessor
    {
        private readonly IUnitOfWork _uow;
        private readonly ISecurityContext _securtiyContext;

        public ExpenseQueryProcessor(IUnitOfWork unitOfWork, ISecurityContext securityContext)
        {
            _securtiyContext = securityContext;
            _uow = unitOfWork;
        }

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
            return GetQuery();
        }

        private IQueryable<Expense> GetQuery()
        {
            var query = _uow.Query<Expense>()
                                        .Where(x => !x.IsDeleted);

            if (!_securtiyContext.IsAdministrator)
            {
                query = query.Where(x => x.UserId == _securtiyContext.User.Id);
            }

            return query;
        }

        public Expense Get(int id)
        {
            var result = GetQuery().Where(i => i.Id == id).FirstOrDefault();

            if (result == null)
                throw new KeyNotFoundException("Expense not found");

            return result;
        }

        public Task<Expense> Update(UpdateExpenseModel updateExpenseModel)
        {
            throw new NotImplementedException();
        }
    }
}
