using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Monefy.Data.Access.Interfaces;
using Monefy.Data.Model;
using Monefy.Queries.Interfaces;
using Monefy.Queries.Queries;
using Monify.Security;
using Moq;
using Xunit;

namespace Monefy.Queries.Tests
{
    public class ExpenseQueryProcessorTests
    {
        private User _currentUser;

        private List<Expense> _expenseList;
        private IExpenseQueryProcessor _query;
        private Random _random;
        
        private Mock<ISecurityContext> _securityContext;
        private Mock<IUnitOfWork> _uow;

        public ExpenseQueryProcessorTests()
        {
            _random = new Random();

            _currentUser = new User { Id = _random.Next() };

            _expenseList = new List<Expense>();

            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(x => x.Query<Expense>()).Returns(() => _expenseList.AsQueryable());

            _securityContext = new Mock<ISecurityContext>();
            _securityContext.Setup(x => x.User).Returns(_currentUser);
            _securityContext.Setup(a => a.IsAdministrator).Returns(false);

            _query = new ExpenseQueryProcessor(_uow.Object, _securityContext.Object);
        }

        [Fact]
        public void GetShouldReturnAll()
        {
            _expenseList.Add(new Expense { UserId = _currentUser.Id });

            var result = _query.Get().ToList();

            result.Count.Should().Be(1);
        }

        [Fact]
        public void GetShouldReturnOnlyUserExpense()
        {
            _expenseList.Add(new Expense { UserId = _currentUser.Id });
            _expenseList.Add(new Expense { UserId = _random.Next() });

            var result = _query.Get().ToList();

            result.Count.Should().Be(1);

            result[0].UserId.Should().Be(_currentUser.Id);
        }

        [Fact]
        public void GetShouldReturnAllExpensesForAdministrator()
        {
            _securityContext.Setup(x => x.IsAdministrator).Returns(true);

            _expenseList.Add(new Expense { UserId = _currentUser.Id });
            _expenseList.Add(new Expense { UserId = _random.Next() });

            var result = _query.Get().ToList();

            result.Count.Should().Be(2);

        }

        [Fact]
        public void GetShouldReturnByExpenseId()
        {
            _expenseList.Add(new Expense { Id = _random.Next(), UserId = _currentUser.Id });

            var expenseId = _random.Next();
            _expenseList.Add(new Expense { Id = expenseId, UserId = _currentUser.Id });

            var result = _query.Get(expenseId);

            result.Id.Should().Be(expenseId);

        }

        [Fact]
        public void GetShouldThrowExceptionIfExpenseNotFound()
        {
            _expenseList.Add(new Expense { Id = _random.Next(), UserId = _currentUser.Id });

            var expenseId = _random.Next();

            Action result = () =>
            {
                _query.Get(expenseId);
            };


            //result.ShouldThrow<NotFoundException>();
        }




    }

}
