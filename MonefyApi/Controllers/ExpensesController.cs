using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monefy.Api.Models.Expenses;
using Monefy.Data.Model;
using Monefy.Queries.Interfaces;
using MonefyApi.Filters;
using MonefyApi.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonefyApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly IExpenseQueryProcessor _expenseQueryProcessor;

        private readonly IAutoMapper _autoMapper;

        public ExpensesController(IExpenseQueryProcessor expenseQueryProcessor, IAutoMapper  autoMapper)
        {
            _expenseQueryProcessor = expenseQueryProcessor;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        public IQueryable<ExpensesModel> Get()
        {
            var result = _expenseQueryProcessor.Get();
            var model = _autoMapper.Map<Expense, ExpensesModel>(result);

            return model;

        }

        [HttpPost]
        public async Task<ExpensesModel> CreateExpense([FromBody] CreateExpenseModel expenseModel)
        {
            var result = await _expenseQueryProcessor.Create(expenseModel);

            var model = _autoMapper.Map<ExpensesModel>(result);

            return model;
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<ExpensesModel> Put(int id, [FromBody]UpdateExpenseModel requestModel)
        {
            var item = await _expenseQueryProcessor.Update(id, requestModel);
            var model = _autoMapper.Map<ExpensesModel>(item);
            return model;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _expenseQueryProcessor.Delete(id);
        }
    }
}
