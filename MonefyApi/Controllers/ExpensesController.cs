using Microsoft.AspNetCore.Mvc;
using Monefy.Api.Models.Expenses;
using Monefy.Data.Model;
using Monefy.Queries.Interfaces;
using MonefyApi.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonefyApi.Controllers
{
    [Route("api/[controller]")]
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


    }
}
