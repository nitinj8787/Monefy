using Monefy.Api.Common;
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

        public IQueryable<Expense> Get()
        {
            return GetQuery();
        }

        private IQueryable<Expense> GetQuery()
        {
            var query = _uow.Query<Expense>().Where(x => !x.IsDeleted);

            if (!_securtiyContext.IsAdministrator)
            {
                query = query.Where(x => x.UserId == _securtiyContext.User.Id);
            }

            return query;
        }


        public async Task<Expense> Create(CreateExpenseModel createExpenseModel)
        {
            var item = new Expense
            {
                Amount = createExpenseModel.Amount,
                Comment = createExpenseModel.Comment,
                Date = createExpenseModel.Date,
                Description = createExpenseModel.Description,

                UserId = _securtiyContext.User.Id
            };

            _uow.Add(item);
            await _uow.CommitAsync();

            return item;
        }

        public async Task Delete(int id)
        {
            var result = GetQuery().Where(i => i.Id == id).FirstOrDefault();

            if (result == null)
                throw new NotFoundException("Expense not found");

            if (result.IsDeleted) return;

            result.IsDeleted = true;

            await _uow.CommitAsync();
        }

        
        public Expense Get(int id)
        {
            var result = GetQuery().Where(i => i.Id == id).FirstOrDefault();

            if (result == null)
                throw new NotFoundException("Expense not found");

            return result;
        }

        public async Task<Expense> Update(int expenseId, UpdateExpenseModel updateExpenseModel)
        {
            var usrData = GetQuery().Where(x => x.Id == expenseId).FirstOrDefault();

            if (usrData == null)
                throw new NotFoundException("Expense not found");


            usrData.Amount = updateExpenseModel.Amount;
            usrData.Comment = updateExpenseModel.Comment;
            usrData.Date = updateExpenseModel.Date;
            usrData.Description = updateExpenseModel.Description;

            await _uow.CommitAsync();

            return usrData;
        }
    }
}
