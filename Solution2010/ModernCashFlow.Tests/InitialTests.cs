using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using NUnit.Framework;

namespace ModernCashFlow.Tests
{
    public class InitialTests
    {
       
        [Test]
        public void Can_calculate_simple_balance()
        {
            var balanceService = new BalanceCalculatorService();

            var incomes = GetIncomes();
            var expenses = GetExpenses();


            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Assert.IsTrue(balance == 0);


        }


        private IEnumerable<Income> GetIncomes()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.01});
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.02});
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.03});

            return incomes;
        }


        private IEnumerable<Expense> GetExpenses()
        {
            var expenses = new List<Expense>();
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.01});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.02});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.03});

            return expenses;
        }




    }
}
